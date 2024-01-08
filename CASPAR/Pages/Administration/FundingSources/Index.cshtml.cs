using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Utility;

namespace CASPAR.Pages.Admin.FundingSources
{
    [Authorize(Roles = SD.AdminRole)]
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public IEnumerable<PayOrg> FundingSources { get; set; }

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            FundingSources = new List<PayOrg>();
        }

        public IActionResult OnGet()
        {
            FundingSources = _unitOfWork.PayOrg.GetAll(payOrg => true, u => u.IsDisabled ? 1 : 0, null);

            return Page();
        }

        public async Task<IActionResult> OnPostLockUnlock(int id) // id passed in is for Amenity getting locked/unlocked
        {
            var payOrg = _unitOfWork.PayOrg.Get(u => u.Id == id);
            if (payOrg.IsDisabled)
            {
                payOrg.IsDisabled = false;
            }
            else
            {
                payOrg.IsDisabled = true;
            }

            _unitOfWork.PayOrg.Update(payOrg);
            await _unitOfWork.CommitAsync();
            return RedirectToPage();
        }
    }
}
