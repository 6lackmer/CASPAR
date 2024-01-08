using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Utility;

namespace CASPAR.Pages.Admin.TimeBlocks
{
    [Authorize(Roles = SD.AdminRole)]
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public IEnumerable<TimeBlock> TimeBlocks { get; set; }

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            TimeBlocks = new List<TimeBlock>();
        }

        public IActionResult OnGet()
        {
            TimeBlocks = _unitOfWork.TimeBlock.GetAll(timeBlock => true, u => u.IsDisabled ? 1 : 0, "TimeBlockType");

            return Page();
        }

        public async Task<IActionResult> OnPostLockUnlock(int id) // id passed in is for Amenity getting locked/unlocked
        {
            var time = _unitOfWork.TimeBlock.Get(u => u.Id == id);
            if (time.IsDisabled)
            {
                time.IsDisabled = false;
            }
            else
            {
                time.IsDisabled = true;
            }

            _unitOfWork.TimeBlock.Update(time);
            await _unitOfWork.CommitAsync();
            return RedirectToPage();
        }
    }
}
