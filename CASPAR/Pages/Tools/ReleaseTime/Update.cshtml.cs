using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Utility;

namespace CASPAR.Pages.Tools.ReleaseTime
{
	[Authorize(Roles = SD.AdminRole)]
	public class UpsertModel : PageModel
	{
		private readonly IUnitOfWork _unitOfWork;

		public Infrastructure.Models.Instructor Instructor { get; set; }

		[BindProperty]
		public Infrastructure.Models.ReleaseTime ReleaseTime { get; set; }

		[BindProperty]
		public Infrastructure.Models.LoadRequirement LoadRequirement { get; set; }

		public UpsertModel(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;

			Instructor = new Infrastructure.Models.Instructor();

			ReleaseTime = new Infrastructure.Models.ReleaseTime();
			LoadRequirement = new Infrastructure.Models.LoadRequirement();
		}

		public IActionResult OnGet(int? loadId)
		{
			if (loadId != null)
			{
				var load = _unitOfWork.LoadRequirement.Get(u => u.Id == (int)loadId);
				if (load != null)
				{
					LoadRequirement = load;

					Instructor = _unitOfWork.Instructor.Get(t => t.Id == LoadRequirement.InstructorId, false, "ApplicationUser");

					ReleaseTime = _unitOfWork.ReleaseTime.Get(u => u.SemesterInstanceId == LoadRequirement.SemesterInstanceId && u.InstructorId == Instructor.Id);

					return Page();
				}
				else
				{
					return NotFound();
				}
			}
			else
			{
				return NotFound();
			}
		}

		public IActionResult OnPostChangeLoadHours()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}
			
			_unitOfWork.LoadRequirement.Update(LoadRequirement);
			_unitOfWork.Commit();

			return RedirectToPage("./Update", new { loadId = LoadRequirement.Id });
		}
		public IActionResult OnPostChangeReleaseTime()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			_unitOfWork.ReleaseTime.Update(ReleaseTime);
			_unitOfWork.Commit();

			var tempLoadId = _unitOfWork.LoadRequirement.Get(u => u.SemesterInstanceId == ReleaseTime.SemesterInstanceId).Id;

			return RedirectToPage("./Update", new { loadId = tempLoadId });
		}
	}
}
