using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Utility;

namespace CASPAR.Pages.Admin.SemesterTypes
{
    [Authorize(Roles = SD.AdminRole)]
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public IEnumerable<Semester> Semesters { get; set; }

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            Semesters = new List<Semester>();
        }

        public IActionResult OnGet()
        {
            Semesters = _unitOfWork.Semester.GetAll(semester => true, u => u.IsDisabled ? 1 : 0, null);

            return Page();
        }

        public async Task<IActionResult> OnPostLockUnlock(int id) // id passed in is for Amenity getting locked/unlocked
        {
            var semester = _unitOfWork.Semester.Get(u => u.Id == id);
            if (semester.IsDisabled)
            {
                semester.IsDisabled = false;
            }
            else
            {
                semester.IsDisabled = true;
            }

            _unitOfWork.Semester.Update(semester);
            await _unitOfWork.CommitAsync();
            return RedirectToPage();
        }
    }
}
