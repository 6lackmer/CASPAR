using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Utility;

namespace CASPAR.Pages.Admin.Buildings
{
    [Authorize(Roles = SD.AdminRole)]
    public class UpsertModel : PageModel
    {

        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public Building Building { get; set; }

        public IEnumerable<SelectListItem> Campuses { get; set; }

        public UpsertModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            var campuses = _unitOfWork.Campus.GetAll(u => u.IsDisabled != true);

            Building = new Building();
            Campuses = campuses.Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.CampusName });
        }

        public IActionResult OnGet(int? id)
        {
            if (id != null)
            {
                var building = _unitOfWork.Building.Get(building => building.Id == id, true, "Campus");

                if (building != null)
                {
                    Building = building;
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

            if (Building.Id == 0)
            {
                _unitOfWork.Building.Add(Building);
            }
            else
            {
                _unitOfWork.Building.Update(Building);
            }

            _unitOfWork.Commit();
            return RedirectToPage("./Index");
        }
    }
}
