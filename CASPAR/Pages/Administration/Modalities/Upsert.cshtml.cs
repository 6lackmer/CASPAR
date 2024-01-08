using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Utility;

namespace CASPAR.Pages.Admin.Modalities
{
    [Authorize(Roles = SD.AdminRole)]
    public class UpsertModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public Modality Modality { get; set; }

        public UpsertModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            Modality = new Modality();
        }

        public IActionResult OnGet(int? id)
        {
            if (id != null)
            {
                var modality = _unitOfWork.Modality.Get(modality => modality.Id == id, true, null);

                if (modality != null)
                {
                    Modality = modality;
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

            if (Modality.Id == 0)
            {
                _unitOfWork.Modality.Add(Modality);
            }
            else
            {
                _unitOfWork.Modality.Update(Modality);
            }

            _unitOfWork.Commit();
            return RedirectToPage("./Index");
        }
    }
}
