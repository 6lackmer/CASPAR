using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Utility;

namespace CASPAR.Pages.Admin.Campuses
{
    [Authorize(Roles = SD.AdminRole)]
    public class IndexModel : PageModel
	{
		private readonly IUnitOfWork _unitOfWork;

		public IEnumerable<Campus> Campuses { get; set; }

		public IndexModel(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;

			Campuses = new List<Campus>();
		}

		public IActionResult OnGet()
		{
			Campuses = _unitOfWork.Campus.GetAll(campus => true, u => u.IsDisabled ? 1 : 0, null);

			return Page();
		}

        public async Task<IActionResult> OnPostLockUnlock(int id) // id passed in is for Amenity getting locked/unlocked
        {
            var campus = _unitOfWork.Campus.Get(u => u.Id == id);
            if (campus.IsDisabled)
            {
                campus.IsDisabled = false;
            }
            else
            {
                campus.IsDisabled = true;
            }

            _unitOfWork.Campus.Update(campus);
            await _unitOfWork.CommitAsync();
            return RedirectToPage();
        }
    }
}
