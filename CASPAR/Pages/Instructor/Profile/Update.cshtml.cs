using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Data;
using System.Security.Claims;
using Utility;

namespace CASPAR.Pages.Instructor.Profile
{
    public class UpdateModel : PageModel
    {
        private IUnitOfWork _unitOfWork;

        [BindProperty]
		public Infrastructure.Models.Instructor Instructor { get; set; }
		public List<AcademicProgram> ProgramList { get; set; }

		public UpdateModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

			ProgramList = _unitOfWork.AcademicProgram.GetAll().ToList();

			Instructor = new Infrastructure.Models.Instructor();
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
                Instructor = _unitOfWork.Instructor.Get(u => u.UserId == claim.Value, false, "ApplicationUser,ProgramAffiliation");
                if (Instructor == null)
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
			if (!ModelState.IsValid)
			{
				return Page();
			}
			else
            {
                // Retrieve Application User Info from Database
                var aUser = _unitOfWork.ApplicationUser.Get(u => u.Id == Instructor.UserId);

                // Populate Changed Information
                aUser.FirstName = Instructor.ApplicationUser.FirstName;
                aUser.LastName = Instructor.ApplicationUser.LastName;

                _unitOfWork.ApplicationUser.Update(aUser);
                _unitOfWork.Commit();

                var wishlists = _unitOfWork.InstructorWishlist.GetAll(u => u.InstructorId == Instructor.Id);
                var sections = _unitOfWork.Section.GetAll(u => u.InstructorId == Instructor.Id);
                var releaseTimes = _unitOfWork.ReleaseTime.GetAll(u => u.InstructorId == Instructor.Id);
                var loads = _unitOfWork.LoadRequirement.GetAll(u => u.InstructorId == Instructor.Id);

                foreach (var w in wishlists)
                {
                    w.InstructorId = 2;
                    _unitOfWork.InstructorWishlist.Update(w);
                }
                foreach (var s in sections)
                {
                    s.InstructorId = 2;
                    _unitOfWork.Section.Update(s);
                }
                foreach (var r in releaseTimes)
                {
                    r.InstructorId = 2;
                    _unitOfWork.ReleaseTime.Update(r);
                }
                foreach (var l in loads)
                {
                    l.InstructorId = 2;
                    _unitOfWork.LoadRequirement.Update(l);
                }

                _unitOfWork.Commit();

                _unitOfWork.Instructor.Remove(Instructor);
                _unitOfWork.Commit();

                var newInstructor = new Infrastructure.Models.Instructor
                {
                    UserId = aUser.Id,
                    InstructorTitle = Instructor.InstructorTitle
                };

                newInstructor.ProgramAffiliation = new List<AcademicProgram>();

                // 2. Program Affiliations
                var programs = Request.Form["Programs"];
                foreach (var p in programs)
                {
                    newInstructor.ProgramAffiliation.Add(_unitOfWork.AcademicProgram.Get(u => u.Id == int.Parse(p)));
                }


                _unitOfWork.Instructor.Add(newInstructor);
                _unitOfWork.Commit();

                foreach (var w in wishlists)
                {
                    w.InstructorId = newInstructor.Id;
                    _unitOfWork.InstructorWishlist.Update(w);
                }
                foreach (var s in sections)
                {
                    s.InstructorId = newInstructor.Id;
                    _unitOfWork.Section.Update(s);
                }
                foreach (var r in releaseTimes)
                {
                    r.InstructorId = newInstructor.Id;
                    _unitOfWork.ReleaseTime.Update(r);
                }
                foreach (var l in loads)
                {
                    l.InstructorId = newInstructor.Id;
                    _unitOfWork.LoadRequirement.Update(l);
                }

                _unitOfWork.Commit();

                return RedirectToPage("./Index");
            }
		}
	}
}
