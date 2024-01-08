using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Utility;

namespace CASPAR.Pages.Admin.Courses
{
    [Authorize(Roles = SD.AdminRole)]
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public IEnumerable<Course> Courses { get; set; }

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            Courses = new List<Course>();
        }

        public IActionResult OnGet()
        {
            Courses = _unitOfWork.Course.GetAll(course => true, u => u.IsDisabled ? 1 : 0, "Program");

            return Page();
        }

        public async Task<IActionResult> OnPostLockUnlock(int id) // id passed in is for Amenity getting locked/unlocked
        {
            var course = _unitOfWork.Course.Get(u => u.Id == id);
            if (course.IsDisabled)
            {
                course.IsDisabled = false;
            }
            else
            {
                course.IsDisabled = true;
            }

            _unitOfWork.Course.Update(course);
            await _unitOfWork.CommitAsync();
            return RedirectToPage();
        }
    }
}
