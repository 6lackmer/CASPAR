using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Utility;

namespace CASPAR.Pages.Schedule
{
	[Authorize(Roles = SD.AdminRole + "," + SD.ProgramCoordinatorRole + "," + SD.FlexCoordinatorRole + "," + SD.GraduateCoordinatorRole + "," + SD.SecretaryRole)]
	public class IndexModel : PageModel
	{
		private readonly IUnitOfWork _unitOfWork;

		public IEnumerable<SemesterInstance> SemesterInstances { get; set; }

		public IndexModel(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;

			SemesterInstances = new List<SemesterInstance>();
		}

		public IActionResult OnGet()
		{
			SemesterInstances = _unitOfWork.SemesterInstance.GetAll(t => true, null, "Semester,SemesterInstanceStatus");
			foreach (var semesterInstance in SemesterInstances)
			{
				semesterInstance.Scheduled = _unitOfWork.Section.GetAll(t => t.SemesterInstanceId == semesterInstance.Id).Count() > 0;
			}

			return Page();
		}
	}
}
