using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Utility;

namespace CASPAR.Pages.Admin.Classrooms
{
    [Authorize(Roles = SD.AdminRole)]
    public class UpsertModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public Classroom Classroom { get; set; }

        public IEnumerable<SelectListItem> Buildings { get; set; }

        public UpsertModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            var buildings = _unitOfWork.Building.GetAll(u => u.IsDisabled != true);

            Classroom = new Classroom();
            Buildings = buildings.Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Name });
        }

        public IActionResult OnGet(int? id)
        {
            if (id != null)
            {
                var classroom = _unitOfWork.Classroom.Get(classroom => classroom.Id == id, true, "Building");

                if (classroom != null)
                {
                    Classroom = classroom;
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

            if (Classroom.Id == 0)
            {
                _unitOfWork.Classroom.Add(Classroom);
            }
            else
            {
                _unitOfWork.Classroom.Update(Classroom);
            }

            _unitOfWork.Commit();
            return RedirectToPage("./Index");
        }
    }
}
