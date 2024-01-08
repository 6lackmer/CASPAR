using DataAccess;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Utility;

namespace CASPAR.Pages.Tools
{
    [Authorize(Roles = SD.AdminRole)]
    public class DetailsModel : PageModel
    {
		private readonly IUnitOfWork _unitOfWork;

		[BindProperty]
		public SemesterInstance SemesterInstance { get; set; }

		[BindProperty]
		public int SemesterStatusId { get; set; } = 0;

		public IEnumerable<SemesterInstanceStatus> SemesterStatuses { get; set; }

		public int InstructorWishlistCourseCount { get; set; } = 0;

		public int StudentWishlistCourseCount { get; set; } = 0;

		public int SectionCount { get; set; } = 0;

		public DetailsModel(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;

			SemesterInstance = new SemesterInstance();
			SemesterStatuses = new List<SemesterInstanceStatus>();
		}

		public IActionResult OnGet(int id)
		{
			var semesterInstance = _unitOfWork.SemesterInstance.Get(semesterInstance => semesterInstance.Id == id, true, "Semester");

			if (semesterInstance != null)
			{
				SemesterInstance = semesterInstance;
				SemesterStatuses = _unitOfWork.SemesterInstanceStatus.GetAll();
				InstructorWishlistCourseCount = _unitOfWork.InstructorWishlistCourse.GetAll(course => course.InstructorWishlist.SemesterInstanceId == semesterInstance.Id).Count();
				StudentWishlistCourseCount = _unitOfWork.StudentWishlistCourse.GetAll(course => course.StudentWishlist.SemesterInstanceId == semesterInstance.Id).Count();
				SectionCount = _unitOfWork.Section.GetAll(section => section.SemesterInstanceId == semesterInstance.Id).Count();
			}
			else
			{
				return NotFound();
			}

			return Page();
		}

		public IActionResult OnPost()
		{
			var semesterStatus = _unitOfWork.SemesterInstanceStatus.Get(SemesterStatusId);

			SemesterInstance = _unitOfWork.SemesterInstance.Get(semesterInstance => semesterInstance.Id == SemesterInstance.Id);
			SemesterInstance.SemesterInstanceStatusId = SemesterStatusId;

			//we need to generate a new schedule when the status moves to 3
			if (SemesterStatusId == 3)
			{
				//check to see that a schedule has not already been generated
				var scheduled = _unitOfWork.Section.GetAll(t => t.SemesterInstanceId == SemesterInstance.Id).Count() > 0;

				if (!scheduled)
				{
					var template = _unitOfWork.Template.Get(t => t.SemesterId == SemesterInstance.SemesterId);
					if (template != null)
					{
						var templateAssignments = _unitOfWork.TemplateAssignment.GetAll(t => t.TemplateId == template.Id);
						if (templateAssignments != null)
						{
							foreach (var assignment in templateAssignments)
							{
								for (int i = 0; i < assignment.FaceToFaceCount; i++)
								{
									_unitOfWork.Section.Add(new Section
									{
										SemesterInstanceId = SemesterInstance.Id,
										CourseId = assignment.CourseId,
										SectionStatusId = 1,
										ModalityId = 4
									});
								}
                                for (int i = 0; i < assignment.OnlineCount; i++)
                                {
                                    _unitOfWork.Section.Add(new Section
                                    {
                                        SemesterInstanceId = SemesterInstance.Id,
                                        CourseId = assignment.CourseId,
                                        SectionStatusId = 1,
                                        ModalityId = 1
                                    });
                                }
                                for (int i = 0; i < assignment.FlexCount; i++)
                                {
                                    _unitOfWork.Section.Add(new Section
                                    {
                                        SemesterInstanceId = SemesterInstance.Id,
                                        CourseId = assignment.CourseId,
                                        SectionStatusId = 1,
                                        ModalityId = 5
                                    });
                                }
                                for (int i = 0; i < assignment.VirtualCount; i++)
                                {
                                    _unitOfWork.Section.Add(new Section
                                    {
                                        SemesterInstanceId = SemesterInstance.Id,
                                        CourseId = assignment.CourseId,
                                        SectionStatusId = 1,
                                        ModalityId = 2
                                    });
                                }
                                for (int i = 0; i < assignment.HybridCount; i++)
                                {
                                    _unitOfWork.Section.Add(new Section
                                    {
                                        SemesterInstanceId = SemesterInstance.Id,
                                        CourseId = assignment.CourseId,
                                        SectionStatusId = 1,
                                        ModalityId = 3
                                    });
                                }
                                for (int i = 0; i < assignment.UnassignedCount; i++)
                                {
                                    _unitOfWork.Section.Add(new Section
                                    {
                                        SemesterInstanceId = SemesterInstance.Id,
                                        CourseId = assignment.CourseId,
                                        SectionStatusId = 1
                                    });
                                }
                            }
							_unitOfWork.Commit();
						}
					}
				}
			}

			_unitOfWork.SemesterInstance.Update(SemesterInstance);

			_unitOfWork.Commit();

			TempData["success"] = $"The status for {SemesterInstance.Name} has changed to {semesterStatus?.Status ?? "Unknown"}.";

			return RedirectToPage("./Index");
		}
	}
}
