using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Utility;

namespace CASPAR.Pages.Admin.PayModels
{
    [Authorize(Roles = SD.AdminRole)]
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public IEnumerable<PayModel> PayModels { get; set; }

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            PayModels = new List<PayModel>();
        }

        public IActionResult OnGet()
        {
            PayModels = _unitOfWork.PayModel.GetAll();

            return Page();
        }

        public async Task<IActionResult> OnPostLockUnlock(int id) // id passed in is for Amenity getting locked/unlocked
        {
            var model = _unitOfWork.PayModel.Get(u => u.Id == id);
            if (model.IsDisabled)
            {
                model.IsDisabled = false;
            }
            else
            {
                model.IsDisabled = true;
            }

            _unitOfWork.PayModel.Update(model);
            await _unitOfWork.CommitAsync();
            return RedirectToPage();
        }
    }
}
