using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Utility;

namespace CASPAR.Pages.Admin.SemesterTypes
{
    [Authorize(Roles = SD.AdminRole)]
    public class UpsertModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public Semester Semester { get; set; }

        public UpsertModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            Semester = new Semester();
        }

        public IActionResult OnGet(int? id)
        {
            if (id != null)
            {
                var semester = _unitOfWork.Semester.Get(semester => semester.Id == id, true, null);

                if (semester != null)
                {
                    Semester = semester;
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

            if (Semester.Id == 0)
            {
                _unitOfWork.Semester.Add(Semester);
            }
            else
            {
                _unitOfWork.Semester.Update(Semester);
            }

            _unitOfWork.Commit();
            return RedirectToPage("./Index");
        }
    }
}
