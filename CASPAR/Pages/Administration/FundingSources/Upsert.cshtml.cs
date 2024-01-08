using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Utility;

namespace CASPAR.Pages.Admin.FundingSources
{
    [Authorize(Roles = SD.AdminRole)]
    public class UpsertModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public PayOrg FundingSource { get; set; }

        public UpsertModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            FundingSource = new PayOrg();
        }

        public IActionResult OnGet(int? id)
        {
            if (id != null)
            {
                var fundingSource = _unitOfWork.PayOrg.Get((int)id);

                if (fundingSource != null)
                {
                    FundingSource = fundingSource;
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

            if (FundingSource.Id == 0)
            {
                _unitOfWork.PayOrg.Add(FundingSource);
            }
            else
            {
                _unitOfWork.PayOrg.Update(FundingSource);
            }

            _unitOfWork.Commit();
            return RedirectToPage("./Index");
        }
    }
}
