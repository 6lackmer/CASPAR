using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Utility;

namespace CASPAR.Pages.Admin.Buildings
{
    [Authorize(Roles = SD.AdminRole)]
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public IEnumerable<Building> Buildings { get; set; }

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            Buildings = new List<Building>();
        }

        public IActionResult OnGet()
        {
			Buildings = _unitOfWork.Building.GetAll(building => building.Campus.IsDisabled != true, u => u.IsDisabled ? 1 : 0, "Campus");

            return Page();
        }

		public async Task<IActionResult> OnPostLockUnlock(int id) // id passed in is for Amenity getting locked/unlocked
		{
			var building = _unitOfWork.Building.Get(u => u.Id == id);
			if (building.IsDisabled)
			{
				building.IsDisabled = false;
			}
			else
			{
				building.IsDisabled = true;
			}

			_unitOfWork.Building.Update(building);
			await _unitOfWork.CommitAsync();
			return RedirectToPage();
		}
	}
}
