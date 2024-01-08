using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Utility;

namespace CASPAR.Pages.Admin.SectionStatuses
{
    [Authorize(Roles = SD.AdminRole)]
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public IEnumerable<SectionStatus> SectionStatuses { get; set; }

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            SectionStatuses = new List<SectionStatus>();
        }

        public IActionResult OnGet()
        {
			SectionStatuses = _unitOfWork.SectionStatus.GetAll();

            return Page();
        }

		public async Task<IActionResult> OnPostLockUnlock(int id) // id passed in is for Status getting locked/unlocked
		{
			var status = _unitOfWork.SectionStatus.Get(u => u.Id == id);
			if (status.IsDisabled)
			{
				status.IsDisabled = false;
			}
			else
			{
				status.IsDisabled = true;
			}

			_unitOfWork.SectionStatus.Update(status);
			await _unitOfWork.CommitAsync();
			return RedirectToPage();
		}
	}
}
