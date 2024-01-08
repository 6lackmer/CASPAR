using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using Utility;

namespace CASPAR.Pages.Administration.Users
{
    using Instructor = Infrastructure.Models.Instructor;
    using Student = Infrastructure.Models.Student;

    public class UpdateModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UpdateModel(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [BindProperty]
        public ApplicationUser AppUser { get; set; }
        public List<string> UserRoles { get; set; }
        public List<string> OldRoles { get; set; }

        public IEnumerable<IdentityRole> AllRoles { get; set; }

        public async void OnGet(string id)
        {
            AppUser = _unitOfWork.ApplicationUser.Get(u => u.Id == id);
            var roles = await _userManager.GetRolesAsync(AppUser);
            UserRoles = roles.ToList();
            OldRoles = roles.ToList();
            AllRoles = _roleManager.Roles;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var newRoles = Request.Form["roles"];
            UserRoles = newRoles.ToList();
            var OldRoles = await _userManager.GetRolesAsync(AppUser); // retrieve ones in db
            var rolesToAdd = new List<string>();
            var user = _unitOfWork.ApplicationUser.Get(u => u.Id == AppUser.Id);

            user.FirstName = AppUser.FirstName;
            user.LastName = AppUser.LastName;
            user.Email = AppUser.Email;
            _unitOfWork.ApplicationUser.Update(user);
            _unitOfWork.Commit();

            //update their roles
            foreach (var r in UserRoles)
            {
                if (!OldRoles.Contains(r))
                {
					if (r == SD.InstructorRole)
					{
                        Instructor tempInstructor = _unitOfWork.Instructor.Get(u => u.UserId == AppUser.Id);
                        if (tempInstructor != null)
                        {
                            tempInstructor.IsDisabled = false;
                            _unitOfWork.Instructor.Update(tempInstructor);
                        }
                        else
                        {
                            tempInstructor = new Instructor()
                            {
                                UserId = AppUser.Id,
                                InstructorTitle = ""
                            };
                            _unitOfWork.Instructor.Add(tempInstructor);
                        }

						_unitOfWork.Commit();
					}
					if (r == SD.StudentRole)
					{
                        Student tempStudent = _unitOfWork.Student.Get(u => u.UserId == AppUser.Id);
                        if (tempStudent != null)
                        {
                            tempStudent.IsDisabled = false;
                            _unitOfWork.Student.Update(tempStudent);
                        }
						else
						{
							tempStudent = new Student()
							{
								UserId = AppUser.Id,
							};
                            _unitOfWork.Student.Add(tempStudent);
						}
						_unitOfWork.Commit();
					}

					rolesToAdd.Add(r);
                }
            }

            foreach (var r in OldRoles)
            {
                if (!UserRoles.Contains(r))
                {
                    if(r == SD.InstructorRole)
                    {
                        Instructor tempInstructor = _unitOfWork.Instructor.Get(u => u.UserId == AppUser.Id);
                        tempInstructor.IsDisabled = true;
                        _unitOfWork.Instructor.Update(tempInstructor);

						_unitOfWork.Commit();
					}
                    if(r == SD.StudentRole)
                    {
                        Student tempStudent = _unitOfWork.Student.Get(u => u.UserId == AppUser.Id);
                        tempStudent.IsDisabled = true;
                        _unitOfWork.Student.Update(tempStudent);

                        _unitOfWork.Commit();
					}

                    var result = await _userManager.RemoveFromRoleAsync(user, r);
                }
            }
            var result1 = await _userManager.AddToRolesAsync(user, rolesToAdd.AsEnumerable());
            return RedirectToPage("./Index", new { success = true, message = "Update Successful" });
        }
    }
}
