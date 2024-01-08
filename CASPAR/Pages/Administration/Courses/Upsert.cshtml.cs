using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Utility;

namespace CASPAR.Pages.Admin.Courses
{
    [Authorize(Roles = SD.AdminRole)]
    public class UpsertModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public Course Course { get; set; }

        public IEnumerable<SelectListItem> Programs { get; set; }

        public UpsertModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            var programs = _unitOfWork.AcademicProgram.GetAll();

            Course = new Course();
            Programs = programs.Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.ProgramName });
        }

        public IActionResult OnGet(int? id)
        {
            if (id != null)
            {
                var course = _unitOfWork.Course.Get(course => course.Id == id, true, "Program");

                if (course != null)
                {
                    Course = course;
                }
                else
                {
                    return NotFound();
                }
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Course.Id == 0)
            {
                _unitOfWork.Course.Add(Course);
            }
            else
            {
                _unitOfWork.Course.Update(Course);
            }

            _unitOfWork.Commit();
            return RedirectToPage("./Index");
        }
    }
}
