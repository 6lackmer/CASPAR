using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Utility;

namespace CASPAR.Pages.Administration.Semesters
{
    [Authorize(Roles = SD.AdminRole)]
    public class UpsertModel : PageModel
    {
		private readonly IUnitOfWork _unitOfWork;

		[BindProperty]
		public SemesterInstance SemesterInstance { get; set; }

        public IEnumerable<Semester> SemesterTypes { get; set; }

        public UpsertModel(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;

			SemesterInstance = new SemesterInstance()
			{
				SemesterInstanceStatusId = 1, //All semesters should start with the "New" status
			};

            SemesterTypes = _unitOfWork.Semester.GetAll(s => !s.IsDisabled, null, null);
		}

		public IActionResult OnGet(int? id)
		{
			if (id != null)
			{
				var semesterInstance = _unitOfWork.SemesterInstance.Get(semester => semester.Id == id, true, null);

				if (semesterInstance != null)
				{
					SemesterInstance = semesterInstance;
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

			if (SemesterInstance.Id == 0)
			{
				_unitOfWork.SemesterInstance.Add(SemesterInstance);

                //add load for all instructors based off the default hours for the semester type
                var semesterType = _unitOfWork.Semester.Get(t => t.Id == SemesterInstance.SemesterId);

				var instructors = _unitOfWork.Instructor.GetAll(t => !t.IsDisabled);

				foreach (var instructor in instructors)
				{
					_unitOfWork.LoadRequirement.Add(new LoadRequirement
					{
						SemesterInstanceId = SemesterInstance.Id,
						InstructorId = instructor.Id,
						LoadReqHours = semesterType?.DefaultLoad ?? 0,
                    });
				}
			}
			else
			{
				_unitOfWork.SemesterInstance.Update(SemesterInstance);
			}

			_unitOfWork.Commit();
			return RedirectToPage("./Index");
		}
	}
}
