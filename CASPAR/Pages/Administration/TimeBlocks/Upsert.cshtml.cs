using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using Utility;

namespace CASPAR.Pages.Admin.TimeBlocks
{
    [Authorize(Roles = SD.AdminRole)]
    public class UpsertModel : PageModel
    {

        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public TimeBlock TimeBlock { get; set; }

        public IEnumerable<SelectListItem> TimeBlockTypes { get; set; }

        public UpsertModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            TimeBlock = new TimeBlock();
            TimeBlockTypes = new List<SelectListItem>();
        }

        public IActionResult OnGet(int? id)
        {
            if (id != null)
            {
                var timeBlock = _unitOfWork.TimeBlock.Get(timeBlock => timeBlock.Id == id, true, "TimeBlockType");


                if (timeBlock != null)
                {
                    var timeBlockTypes = _unitOfWork.TimeBlockType.GetAll();

                    TimeBlock = timeBlock;
                    TimeBlockTypes = timeBlockTypes.Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Name });
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

            if (TimeBlock.Id == 0)
            {
                _unitOfWork.TimeBlock.Add(TimeBlock);
            }
            else
            {
                _unitOfWork.TimeBlock.Update(TimeBlock);
            }

            _unitOfWork.Commit();
            return RedirectToPage("./Index");
        }
    }
}
