using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Utility;

namespace CASPAR.Pages.Tools
{
    [Authorize(Roles = SD.AdminRole)]
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public IEnumerable<SemesterInstance> SemesterInstances { get; set; }

        public IEnumerable<SemesterInstanceStatus> SemesterStatuses { get; set; }

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            SemesterInstances = new List<SemesterInstance>();
            SemesterStatuses = new List<SemesterInstanceStatus>();
        }

        public IActionResult OnGet()
        {
            SemesterInstances = _unitOfWork.SemesterInstance.GetAll(t => true, null, "Semester,SemesterInstanceStatus");
            foreach (var semesterInstance in SemesterInstances)
            {
				semesterInstance.Scheduled = _unitOfWork.Section.GetAll(t => t.SemesterInstanceId == semesterInstance.Id).Count() > 0;
			}

            SemesterStatuses = _unitOfWork.SemesterInstanceStatus.GetAll();

            return Page();
        }
    }
}
