using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Utility;

namespace CASPAR.Pages.Admin.Campuses
{
    [Authorize(Roles = SD.AdminRole)]
    public class UpsertModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public Campus Campus { get; set; }

        public UpsertModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            Campus = new Campus();
        }

        public IActionResult OnGet(int? id)
        {
            if (id != null)
            {
                var campus = _unitOfWork.Campus.Get(campus => campus.Id == id, true, null);

                if (campus != null)
                {
                    Campus = campus;
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

            if (Campus.Id == 0)
            {
                _unitOfWork.Campus.Add(Campus);
            }
            else
            {
                _unitOfWork.Campus.Update(Campus);
            }

            _unitOfWork.Commit();
            return RedirectToPage("./Index");
        }
    }
}
