using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Cryptography;
using Utility;

namespace CASPAR.Pages.Tools.ReleaseTime
{
	[Authorize(Roles = SD.AdminRole)]
	public class IndexModel : PageModel
	{
		private readonly IUnitOfWork _unitOfWork;

		[BindProperty]
		public Infrastructure.Models.Instructor Instructor { get; set; }

		[BindProperty]
		public SemesterInstance Semester { get; set; }

		public IEnumerable<SemesterInstance> Semesters { get; set; }

		[BindProperty]
		public IEnumerable<Infrastructure.Models.ReleaseTime> ReleaseTimes { get; set; }

		[BindProperty]
		public LoadRequirement LoadRequirement { get; set; }

		public IndexModel(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;

			Instructor = new Infrastructure.Models.Instructor();

			ReleaseTimes = new List<Infrastructure.Models.ReleaseTime>();
			LoadRequirement = new LoadRequirement();
			Semester = new SemesterInstance();
			Semesters = _unitOfWork.SemesterInstance.GetAll(u => true, null, "Semester");
		}

		public IActionResult OnGet(int? instructorId, int? semesterSearchId)
		{
			if (instructorId == null)
			{
				return RedirectToPage("/Error");
			}

			var instructor = _unitOfWork.Instructor.Get(u => u.Id == (int)instructorId, false, "ApplicationUser");

			if (instructor == null)
			{
				return RedirectToPage("/Error");
			}

			Instructor = instructor;

			SemesterInstance? semester;

			if (semesterSearchId != null)
			{
				semester = _unitOfWork.SemesterInstance.Get(u => u.Id == semesterSearchId);
			}
			else // Pull release times for the latest semester
			{
				semester = _unitOfWork.SemesterInstance.GetAll().OrderBy(t => t.SemesterStartDate).Last();
			}

			if (semester == null)
			{
				return RedirectToPage("/Error");
			}

			Semester = semester;

			var semesterLoad = _unitOfWork.LoadRequirement.Get(u => u.InstructorId == Instructor.Id && u.SemesterInstanceId == Semester.Id, false, "SemesterInstance,SemesterInstance.Semester");
			if (semesterLoad == null)
			{
				semesterLoad = new LoadRequirement
				{
					SemesterInstanceId = Semester.Id,
					InstructorId = Instructor.Id,
					LoadReqHours = 12,
				};
				_unitOfWork.LoadRequirement.Add(semesterLoad);
				_unitOfWork.Commit();
			}

			LoadRequirement = semesterLoad;
			ReleaseTimes = _unitOfWork.ReleaseTime.GetAll(u => u.InstructorId == Instructor.Id && u.SemesterInstanceId == Semester.Id);

			return Page();
		}

		public IActionResult OnPost()
		{
			//can't use regular form validation with the release time fields
			//if (!ModelState.IsValid)
			//{
			//	return Page();
			//}

			_unitOfWork.LoadRequirement.Update(LoadRequirement);
			foreach (var releaseTime in ReleaseTimes)
			{
				_unitOfWork.ReleaseTime.Update(releaseTime);
			}
			_unitOfWork.Commit();

			return RedirectToPage("./Index", new { instructorId = Instructor.Id, semesterSearchId = Semester.Id });
		}

		public IActionResult OnPostAddReleaseTime()
		{
			_unitOfWork.ReleaseTime.Add(new Infrastructure.Models.ReleaseTime
			{
				SemesterInstanceId = Semester.Id,
				InstructorId = Instructor.Id,
			});
			_unitOfWork.Commit();

			return RedirectToPage("./Index", new { instructorId = Instructor.Id, semesterSearchId = Semester.Id });
		}

		public IActionResult OnPostRemoveReleaseTime(int id)
		{
			var releaseTime = _unitOfWork.ReleaseTime.Get(id);

			if (releaseTime != null)
			{
				_unitOfWork.ReleaseTime.Remove(releaseTime);
				_unitOfWork.Commit();
			}

			return RedirectToPage("./Index", new { instructorId = Instructor.Id, semesterSearchId = Semester.Id });
		}
	}
}