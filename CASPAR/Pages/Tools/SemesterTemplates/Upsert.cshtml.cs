using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Utility;

namespace CASPAR.Pages.Tools.SemesterTemplates
{
    [Authorize(Roles = SD.AdminRole)]
    public class UpsertModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public TemplateAssignment Course { get; set; }
        public IEnumerable<SelectListItem> Courses { get; set; }
        public IEnumerable<SelectListItem> UsedCourses { get; set; }
        public IEnumerable<SelectListItem> AllCourses { get; set; }

        public UpsertModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            var allCourses = _unitOfWork.Course.GetAll(u => u.IsDisabled == false);

            foreach (var course in allCourses)
            {
                course.Program = _unitOfWork.AcademicProgram.Get(u => u.Id == course.ProgramId);
            }

            Course = new TemplateAssignment();

            AllCourses = allCourses.Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Program.ProgramCode + " " + t.CourseNumber });
        }

        public IActionResult OnGet(int? templateId, int? courseId) // There will be both Template and Course Ids passed into here.
        {
            if (templateId == null)
            {
                return NotFound();
            }
            else
            {
                if (courseId == null || courseId == 0)
                {
                    Course.TemplateId = (int)templateId;

                    // Filter Course list down to just courses that aren't
                    // currently in the template

                    var usedCourses = _unitOfWork.TemplateAssignment.GetAll(u => u.TemplateId == templateId, null, "Course");
                    foreach (var c in usedCourses)
                    {
                        c.Course.Program = _unitOfWork.AcademicProgram.Get(u => u.Id == c.Course.ProgramId);
                    }

                    UsedCourses = usedCourses.Select(t => new SelectListItem { Value = t.CourseId.ToString(), Text = t.Course.Program.ProgramCode + " " + t.Course.CourseNumber });

                    List<SelectListItem> CoursesList = new List<SelectListItem>();
                    foreach (var c in AllCourses)
                    {
                        bool courseUsed = false;
                        foreach (var d in UsedCourses)
                        {
                            if (c.Value == d.Value)
                            {
                                courseUsed = true;
                            }
                        }
                        if (courseUsed == false)
                        {
                            CoursesList.Add(c);
                        }
                    }

                    Courses = CoursesList;


                    return Page();
                }
                else
                {
                    Course = _unitOfWork.TemplateAssignment.Get(u => u.Id == courseId && u.TemplateId == templateId, false, "Course");

                    Course.Course.Program = _unitOfWork.AcademicProgram.Get(u => u.Id == Course.Course.ProgramId);

                    return Page();
                }
            }
        }

        public IActionResult OnPost(int templateId, int courseId)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToPage("./Upsert", new { templateId, courseId});
            }
            if(Course.TotalCount < (Course.VirtualCount + Course.OnlineCount + Course.FaceToFaceCount + Course.FlexCount + Course.HybridCount))
            {
                TempData["Error"] = "Total Quantity must exceed the Sum of the Rest!";
                return RedirectToPage("./Upsert", new { templateId, courseId });
            }
            Course.TemplateId = templateId;

            if(Course.Id == 0)
            {
                _unitOfWork.TemplateAssignment.Add(Course);
            }
            else
            {
                _unitOfWork.TemplateAssignment.Update(Course);
            }

            _unitOfWork.Commit();
            return RedirectToPage("./Index");
        }
    }
}