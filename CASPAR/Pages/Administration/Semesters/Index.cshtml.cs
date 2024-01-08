using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Utility;

namespace CASPAR.Pages.Administration.Semesters
{
    [Authorize(Roles = SD.AdminRole)]
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

			return Page();
		}
	}
}
