using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Utility;

namespace CASPAR.Pages.Admin.Amenities
{
    [Authorize(Roles = SD.AdminRole)]
    public class UpsertModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public ClassroomFeature Amenity { get; set; }

        public UpsertModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            Amenity = new ClassroomFeature();
        }

        public IActionResult OnGet(int? id)
        {
            if (id != null)
            {
                var amenity = _unitOfWork.ClassroomFeature.Get((int)id);

                if (amenity != null)
                {
                    Amenity = amenity;
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

            if (Amenity.Id == 0)
            {
                _unitOfWork.ClassroomFeature.Add(Amenity);
            }
            else
            {
                _unitOfWork.ClassroomFeature.Update(Amenity);
            }

            _unitOfWork.Commit();
            return RedirectToPage("./Index");
        }
    }
}
