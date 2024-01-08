using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Utility;

namespace CASPAR.Pages.Student.Wishlist
{
    [Authorize(Roles = SD.StudentRole)]
    public class UpsertModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public StudentWishlistCourse Course { get; set; }

        public IEnumerable<SelectListItem> CourseList { get; set; }
        public IEnumerable<SelectListItem> UsedCourses { get; set; }
        public IEnumerable<SelectListItem> AllCoursesFormatted { get; set; }
        public IEnumerable<Course> AllCourses { get; set; }
        public List<Modality> ModalityList { get; set; }
        public List<Campus> CampusList { get; set; }
        public List<TimeBlock> TimeBlockList { get; set; }
        private readonly int TotalCredits; // total number of credits a student can take in a semester

        public UpsertModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            var studentTimeBlock = _unitOfWork.TimeBlockType.Get(t => t.Name == "Student");

            Course = new StudentWishlistCourse();

            AllCourses = _unitOfWork.Course.GetAll(u => u.IsDisabled == false);
            foreach (var c in AllCourses)
            {
                c.Program = _unitOfWork.AcademicProgram.Get(u => u.Id == c.ProgramId);
            }
            AllCoursesFormatted = AllCourses.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Program.ProgramCode + " " + c.CourseNumber + " (" + c.CreditHours + ")" });

            ModalityList = _unitOfWork.Modality.GetAll(u => u.IsDisabled != true).ToList();
            CampusList = _unitOfWork.Campus.GetAll(u => u.IsDisabled != true).ToList();
            
            TimeBlockList = _unitOfWork.TimeBlock.GetAll(u => u.TimeBlockType == studentTimeBlock && u.IsDisabled != true).ToList();
            TotalCredits = 20; //TODO: Maybe change this value to be dynamic
        }

        public IActionResult OnGet(int? wishlistId, int? id)
        {
            #region Updating a Course
            if (id != null)
            {
                var course = _unitOfWork.StudentWishlistCourse.Get(t => t.Id == (int)id, false, "Course,Modalities,TimeBlocks,Campuses");
                course.Course.Program = _unitOfWork.AcademicProgram.Get(t => t.Id == course.Course.ProgramId);

                if (course != null)
                {
                    Course = course;
                }
                else
                {
                    return NotFound();
                }
            }
            #endregion

            #region Adding a course to a wishlist
            else if (wishlistId != null)
            {
                Course.StudentWishlistId = (int)wishlistId;

                // All courses on wishlist
                var usedCourses = _unitOfWork.StudentWishlistCourse.GetAll(u => u.StudentWishlistId == Course.StudentWishlistId, null, "Course");

                #region Populate Used Courses
                var TotalCreditsPicked = 0;

                foreach (var c in usedCourses)
                {
                    c.Course.Program = _unitOfWork.AcademicProgram.Get(u => u.Id == c.Course.ProgramId);
                    TotalCreditsPicked += c.Course.CreditHours;
                }
                #endregion

                UsedCourses = usedCourses.Select(t => new SelectListItem { Value = t.CourseId.ToString(), Text = t.Course.Program.ProgramCode + " " + t.Course.CourseNumber });
                List<SelectListItem> Courses = new List<SelectListItem>();

                int remainingCredits = TotalCredits - TotalCreditsPicked;

                #region Filter Courses Not Used in Wishlist
                foreach (var c in AllCoursesFormatted)
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
                        // if the course credits is less than or equal to the remaining credits, add it to the list
                        var course = AllCourses.Where(u => u.Id == int.Parse(c.Value)).FirstOrDefault();
                        if (course.CreditHours <= remainingCredits)
                        {
                            Courses.Add(c);
                        }
                    }
                }
                #endregion

                // prevents access to the page if there are no courses available to pick 
                if (Courses.Count() == 0)
                    return RedirectToPage("./Index", new { wishlistSearchId = wishlistId.ToString() });

                CourseList = Courses;
            }
            #endregion
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Course.Id == 0) // this is a new course
            {

                Course.Modalities = new List<Modality>();
                Course.Campuses = new List<Campus>();
                Course.TimeBlocks = new List<TimeBlock>();

                // Attributes to Handle
                // 1. CourseId should be bound via the form

                // 2. StudentWishlistId
                // WishlistID should be bound via the form

                // 3. ModalityTypes
                var modalities = Request.Form["Modalities"];
                foreach (var m in modalities)
                {
                    Course.Modalities.Add(_unitOfWork.Modality.Get(u => u.Id == int.Parse(m)));
                }

                // 4. TimeBlocks
                var timeBlocks = Request.Form["Times"];
                foreach (var t in timeBlocks)
                {
                    Course.TimeBlocks.Add(_unitOfWork.TimeBlock.Get(u => u.Id == int.Parse(t)));
                }

                // 5. Campuses
                var campus = Request.Form["Campuses"];
                foreach (var c in campus)
                {
                    Course.Campuses.Add(_unitOfWork.Campus.Get(u => u.Id == int.Parse(c)));
                }

                _unitOfWork.StudentWishlistCourse.Add(Course);
            }
            else
            {
                _unitOfWork.StudentWishlistCourse.Remove(Course);
                _unitOfWork.Commit();

                Course.Id = 0; // So that Db doesn't crash
                Course.Modalities = new List<Modality>();
                Course.Campuses = new List<Campus>();
                Course.TimeBlocks = new List<TimeBlock>();

                // 2. ModalityTypes
                var modalities = Request.Form["Modalities"];
                foreach (var m in modalities)
                {
                    Course.Modalities.Add(_unitOfWork.Modality.Get(u => u.Id == int.Parse(m)));
                }

                // 3. TimeBlocks
                var timeBlocks = Request.Form["Times"];
                foreach (var t in timeBlocks)
                {
                    Course.TimeBlocks.Add(_unitOfWork.TimeBlock.Get(u => u.Id == int.Parse(t)));
                }

                // 4. Campuses
                var campus = Request.Form["Campuses"];
                foreach (var c in campus)
                {
                    Course.Campuses.Add(_unitOfWork.Campus.Get(u => u.Id == int.Parse(c)));
                }

                _unitOfWork.StudentWishlistCourse.Add(Course);
            }

            _unitOfWork.Commit();
            return RedirectToPage("./Index", new { wishlistSearchId = Course.StudentWishlistId.ToString() });
        }
    }
}
