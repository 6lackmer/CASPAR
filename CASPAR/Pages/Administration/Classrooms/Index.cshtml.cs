using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Utility;

namespace CASPAR.Pages.Admin.Classrooms
{
    [Authorize(Roles = SD.AdminRole)]
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public IEnumerable<Classroom> Classrooms { get; set; }

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            Classrooms = new List<Classroom>();
        }

        public IActionResult OnGet()
        {
            Classrooms = _unitOfWork.Classroom.GetAll(classroom => classroom.Building.IsDisabled != true, u => u.IsDisabled ? 1 : 0, "Building,Features");

            return Page();
        }

        public async Task<IActionResult> OnPostLockUnlock(int id) // id passed in is for Amenity getting locked/unlocked
        {
            var classroom = _unitOfWork.Classroom.Get(u => u.Id == id);
            if (classroom.IsDisabled)
            {
                classroom.IsDisabled = false;
            }
            else
            {
                classroom.IsDisabled = true;
            }

            _unitOfWork.Classroom.Update(classroom);
            await _unitOfWork.CommitAsync();
            return RedirectToPage();
        }
    }
}
