using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Utility;

namespace CASPAR.Pages.Admin.PartsOfTerm
{
    [Authorize(Roles = SD.AdminRole)]
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public IEnumerable<PartOfTerm> PartsOfTerm { get; set; }

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            PartsOfTerm = new List<PartOfTerm>();
        }

        public IActionResult OnGet()
        {
            PartsOfTerm = _unitOfWork.PartOfTerm.GetAll(partOfTerm => true, u => u.IsDisabled ? 1 : 0, null);

            return Page();
        }

        public async Task<IActionResult> OnPostLockUnlock(int id) // id passed in is for Amenity getting locked/unlocked
        {
            var part = _unitOfWork.PartOfTerm.Get(u => u.Id == id);
            if (part.IsDisabled)
            {
                part.IsDisabled = false;
            }
            else
            {
                part.IsDisabled = true;
            }

            _unitOfWork.PartOfTerm.Update(part);
            await _unitOfWork.CommitAsync();
            return RedirectToPage();
        }
    }
}
