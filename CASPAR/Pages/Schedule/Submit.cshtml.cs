using DataAccess;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Utility;

namespace CASPAR.Pages.Schedule
{
    [Authorize(Roles = SD.AdminRole + "," + SD.ProgramCoordinatorRole + "," + SD.FlexCoordinatorRole + "," + SD.GraduateCoordinatorRole + "," + SD.SecretaryRole)]
    public class SubmitModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public SemesterInstance SemesterInstance { get; set; } = new();

        public int SectionCount { get; set; }

        public int CreditHours { get; set; }

		public string ReturnRoute { get; set; } = "index";

		public SubmitModel(ApplicationDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public IActionResult OnGet(int id, string returnRoute)
        {
			ReturnRoute = returnRoute;

			var semesterInstance = _unitOfWork.SemesterInstance.Get(t => t.Id == id, false, "Semester,SemesterInstanceStatus");

            if (semesterInstance != null)
            {
                SemesterInstance = semesterInstance;

                var creditHours = _context.Sections.Where(t => t.SemesterInstanceId == SemesterInstance.Id).Include("Course").Select(t => t.Course.CreditHours).Sum();
                var assignedCreditHours = _context.Sections.Where(t =>
                    t.SemesterInstanceId == SemesterInstance.Id &&
                    t.InstructorId != null &&
                    t.ModalityId != null &&
                    t.PartOfTermId != null &&
                    (!t.Modality.HasCampuses || (t.Modality.HasCampuses && t.ClassroomId != null)) &&
                    (!t.Modality.HasTimeBlocks || (t.Modality.HasTimeBlocks && t.DaysOfWeek != string.Empty && t.SectionTime != null)) &&
                    (t.SectionPays != null && t.SectionPays.Count > 0)
                ).Include("Course").Select(t => t.Course.CreditHours).Sum();

                if (creditHours != assignedCreditHours)
                {
                    return RedirectToPage("/Error");
                }

                SectionCount = _unitOfWork.Section.GetAll(section => section.SemesterInstanceId == semesterInstance.Id).Count();
                CreditHours = creditHours;
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            SemesterInstance = _unitOfWork.SemesterInstance.Get(semesterInstance => semesterInstance.Id == SemesterInstance.Id);
            SemesterInstance.SemesterInstanceStatusId++;

            _unitOfWork.SemesterInstance.Update(SemesterInstance);

            _unitOfWork.Commit();

            return RedirectToPage("./Index");
        }
    }
}
