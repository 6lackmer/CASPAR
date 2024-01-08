using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Utility;

namespace CASPAR.Pages.Admin.Amenities
{
    [Authorize(Roles = SD.AdminRole)]
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public IEnumerable<ClassroomFeature> Amenities { get; set; }

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            Amenities = new List<ClassroomFeature>();
        }

        public IActionResult OnGet()
        {
            Amenities = _unitOfWork.ClassroomFeature.GetAll(feature => true, u => u.IsDisabled ? 1 : 0, null);

            return Page();
        }

        public async Task<IActionResult> OnPostLockUnlock(int id) // id passed in is for Amenity getting locked/unlocked
        {
            var feature = _unitOfWork.ClassroomFeature.Get(u => u.Id == id);
            if (feature.IsDisabled)
            {
                feature.IsDisabled = false;
            }
            else
            {
                feature.IsDisabled = true;
            }

            _unitOfWork.ClassroomFeature.Update(feature);
            await _unitOfWork.CommitAsync();
            return RedirectToPage();
        }
    }
}
