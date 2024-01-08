using CsvHelper.Configuration.Attributes;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Utility;

namespace CASPAR.Pages.Tools.SemesterTemplates
{
    [Authorize(Roles = SD.AdminRole)]
    public class IndexModel : PageModel
	{
		private readonly IUnitOfWork _unitOfWork;

		public Template Template { get; set; }
		public IEnumerable<TemplateAssignment> CourseList { get; set; }

		public IndexModel(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;

			Template = new Template();
		}

		public void OnGet(int? id = 1)
		{
			Template = _unitOfWork.Template.Get(u => u.Id == id);
			Template.Semester = _unitOfWork.Semester.Get(u => u.Id == Template.SemesterId);

			CourseList = _unitOfWork.TemplateAssignment.GetAll(u => u.TemplateId == Template.Id, null, "Course");

			foreach (var c in CourseList)
			{
				c.Course.Program = _unitOfWork.AcademicProgram.Get(u => u.Id == c.Course.ProgramId);
			}
		}

		public IActionResult OnPostMinus(int courseId)
		{
			var course = _unitOfWork.TemplateAssignment.Get(c => c.Id == courseId);
			if (course.TotalCount == 1)
			{
				_unitOfWork.TemplateAssignment.Remove(course);
			}
			else
			{
				// Lower Count
				course.TotalCount -= 1;

				// Lower count of a modality, if there are more modalities than total
				if(course.TotalCount < (course.FlexCount + course.OnlineCount + course.FaceToFaceCount + course.VirtualCount + course.HybridCount))
				{
					var diminish = course.FaceToFaceCount;
					string count = "Fa";
					if(course.OnlineCount > diminish)
					{
						count = "O";
					}
                    if (course.FlexCount > diminish)
                    {
						count = "Fl";
                    }
                    if (course.VirtualCount > diminish)
                    {
						count = "V";
                    }
                    if (course.HybridCount > diminish)
                    {
						count = "H";
                    }

					if(count == "Fa")
					{
						course.FaceToFaceCount -= 1;
					}
					else if(count == "O")
					{
						course.OnlineCount -= 1;
					}
					else if(count == "Fl")
					{
						course.FlexCount -= 1;
					}
					else if(count == "V")
					{
						course.VirtualCount -= 1;
					}
					else if(count == "H")
					{
						course.HybridCount -= 1;
					}
                }

				_unitOfWork.TemplateAssignment.Update(course);
			}
			_unitOfWork.Commit();

			return RedirectToPage("/Tools/SemesterTemplates/Index", new { id = course.TemplateId });
		} // End OnPostMinus

		public IActionResult OnPostPlus(int courseId)
		{
			var course = _unitOfWork.TemplateAssignment.Get(c => c.Id == courseId);

			course.TotalCount += 1;
			_unitOfWork.TemplateAssignment.Update(course);

			_unitOfWork.Commit();

			return RedirectToPage("/Tools/SemesterTemplates/Index", new { id = course.TemplateId });
		} // End OnPostPlus
	}
}