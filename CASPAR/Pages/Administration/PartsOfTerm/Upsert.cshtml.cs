using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Utility;

namespace CASPAR.Pages.Admin.PartsOfTerm
{
    [Authorize(Roles = SD.AdminRole)]
    public class UpsertModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public PartOfTerm PartOfTerm { get; set; }

        public UpsertModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            PartOfTerm = new PartOfTerm();
        }

        public IActionResult OnGet(int? id)
        {
            if (id != null)
            {
                var partOfTerm = _unitOfWork.PartOfTerm.Get(partOfTerm => partOfTerm.Id == id, true, null);

                if (partOfTerm != null)
                {
                    PartOfTerm = partOfTerm;
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

            if (PartOfTerm.Id == 0)
            {
                _unitOfWork.PartOfTerm.Add(PartOfTerm);
            }
            else
            {
                _unitOfWork.PartOfTerm.Update(PartOfTerm);
            }

            _unitOfWork.Commit();
            return RedirectToPage("./Index");
        }
    }
}
