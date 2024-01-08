using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace CASPAR.Pages.Student.Profile
{
    public class UpdateModel : PageModel
    {
        private IUnitOfWork _unitOfWork;

		[BindProperty]
		public Infrastructure.Models.Student Student { get; set; }
        public IEnumerable<SelectListItem> Majors { get; set; }

        public UpdateModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            var majors = _unitOfWork.Major.GetAll();
            Majors = majors.Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.MajorName });

            Student = new Infrastructure.Models.Student();
        }

        public IActionResult OnGet()
        {
            #region Verify User is logged in 
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                return NotFound();
            }
            else
            {
                Student = _unitOfWork.Student.Get(u => u.UserId == claim.Value, false, "User,Major");
                if (Student == null)
                {
                    return NotFound();
                }
                else
                {
                    return Page();
                }
            }
            #endregion
        }

		public IActionResult OnPost()
		{
            if (ModelState.IsValid)
            {
                var aUser = _unitOfWork.ApplicationUser.Get(u => u.Id == Student.UserId);

                aUser.FirstName = Student.User.FirstName;
                aUser.LastName = Student.User.LastName;

                _unitOfWork.Student.Update(Student);
                _unitOfWork.ApplicationUser.Update(aUser);
                _unitOfWork.Commit();

                return RedirectToPage("./Index");
            }
            else
            {
                return Page();
            }
		}
	}
}
