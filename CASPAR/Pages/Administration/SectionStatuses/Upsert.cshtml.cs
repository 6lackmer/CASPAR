using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Utility;

namespace CASPAR.Pages.Admin.SectionStatuses
{
    [Authorize(Roles = SD.AdminRole)]
    public class UpsertModel : PageModel
    {

        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public SectionStatus SectionStatus { get; set; }

        public List<SelectListItem> Colors { get; set; }

        public UpsertModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            var campuses = _unitOfWork.Campus.GetAll(u => u.IsDisabled != true);

            SectionStatus = new SectionStatus();
            Colors = new List<SelectListItem>() {
                new SelectListItem { Text = "Gray", Value = "gray" },
                new SelectListItem { Text = "Purple", Value = "purple" },
                new SelectListItem { Text = "Blue", Value = "blue" },
                new SelectListItem { Text = "Green", Value = "green" },
                new SelectListItem { Text = "Yellow", Value = "yellow" },
                new SelectListItem { Text = "Orange", Value = "orange" },
                new SelectListItem { Text = "Red", Value = "red" }
            };
        }

        public IActionResult OnGet(int? id)
        {
            if (id != null)
            {
                var status = _unitOfWork.SectionStatus.Get(status => status.Id == id);

                if (status != null)
                {
                    SectionStatus = status;
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

            if (SectionStatus.Id == 0)
            {
                _unitOfWork.SectionStatus.Add(SectionStatus);
            }
            else
            {
                _unitOfWork.SectionStatus.Update(SectionStatus);
            }

            _unitOfWork.Commit();
            return RedirectToPage("./Index");
        }
    }
}
