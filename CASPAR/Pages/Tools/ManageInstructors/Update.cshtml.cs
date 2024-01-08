using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Utility;

namespace CASPAR.Pages.Tools.ManageInstructors
{
    [Authorize(Roles = SD.AdminRole)]
    public class UpsertModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public Infrastructure.Models.Instructor Instructor { get; set; }

        public List<AcademicProgram> ProgramList { get; set; }

        public UpsertModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            Instructor = new Infrastructure.Models.Instructor();

            ProgramList = _unitOfWork.AcademicProgram.GetAll().ToList();
        }

        public IActionResult OnGet(string id)
        {
            if (id != null)
            {
                var instructor = _unitOfWork.Instructor.Get(t => t.UserId == id, false, "ApplicationUser,ProgramAffiliation");

                if (instructor != null)
                {
                    Instructor = instructor;
                }

                return Page();
            }
            else
            {
                return NotFound();
            }
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            else
            {
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

				Instructor.ProgramAffiliation = new List<AcademicProgram>();

                // 2. Program Affiliations
                var programs = Request.Form["Programs"];
                foreach (var p in programs)
                {
                    Instructor.ProgramAffiliation.Add(_unitOfWork.AcademicProgram.Get(u => u.Id == int.Parse(p)));
                }


				Instructor.Id = 0;

				_unitOfWork.Instructor.Add(Instructor);
                _unitOfWork.Commit();

                foreach(var w in wishlists)
                {
                    w.InstructorId = Instructor.Id;
                    _unitOfWork.InstructorWishlist.Update(w);
                }
                foreach (var s in sections)
                {
                    s.InstructorId = Instructor.Id;
                    _unitOfWork.Section.Update(s);
                }
                foreach (var r in releaseTimes)
                {
                    r.InstructorId = Instructor.Id;
                    _unitOfWork.ReleaseTime.Update(r);
                }
                foreach (var l in loads)
                {
                    l.InstructorId = Instructor.Id;
                    _unitOfWork.LoadRequirement.Update(l);
                }

                _unitOfWork.Commit();

                return RedirectToPage("./Index");
            }
        }
    }
}
