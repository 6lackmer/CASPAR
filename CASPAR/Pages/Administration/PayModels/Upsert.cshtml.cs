using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Utility;

namespace CASPAR.Pages.Admin.PayModels
{
    [Authorize(Roles = SD.AdminRole)]
    public class UpsertModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public PayModel PayModel { get; set; }

        public UpsertModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            PayModel = new PayModel();
        }

        public IActionResult OnGet(int? id)
        {
            if (id != null)
            {
                var payModel = _unitOfWork.PayModel.Get((int)id);

                if (payModel != null)
                {
                    PayModel = payModel;
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

            if (PayModel.Id == 0)
            {
                _unitOfWork.PayModel.Add(PayModel);
            }
            else
            {
                _unitOfWork.PayModel.Update(PayModel);
            }

            _unitOfWork.Commit();
            return RedirectToPage("./Index");
        }
    }
}
