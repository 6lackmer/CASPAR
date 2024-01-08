using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Utility;

namespace CASPAR.Pages.Admin.Modalities
{
    [Authorize(Roles = SD.AdminRole)]
    public class IndexModel : PageModel
	{
		private readonly IUnitOfWork _unitOfWork;

		public IEnumerable<Modality> Modalities { get; set; }

		public IndexModel(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;

			Modalities = new List<Modality>();
		}

		public IActionResult OnGet()
		{
			Modalities = _unitOfWork.Modality.GetAll(modality => true, u => u.IsDisabled ? 1 : 0, null);

			return Page();
		}


        public async Task<IActionResult> OnPostLockUnlock(int id) // id passed in is for Amenity getting locked/unlocked
        {
            var modality = _unitOfWork.Modality.Get(u => u.Id == id);
            if (modality.IsDisabled)
            {
                modality.IsDisabled = false;
            }
            else
            {
                modality.IsDisabled = true;
            }

            _unitOfWork.Modality.Update(modality);
            await _unitOfWork.CommitAsync();
            return RedirectToPage();
        }
    }
}
