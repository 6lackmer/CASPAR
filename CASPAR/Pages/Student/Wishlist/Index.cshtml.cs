using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Utility;

namespace CASPAR.Pages.Student.Wishlist
{
	[Authorize(Roles = SD.StudentRole)]
	public class IndexModel : PageModel
	{
		private readonly IUnitOfWork _unitOfWork;
		public StudentWishlist StudentWishlist { get; set; }
		public IEnumerable<StudentWishlist> StudentWishlists { get; set; }
		public IEnumerable<StudentWishlistCourse> StudentWishlistCourse { get; set; }

		public bool AllowUpsert { get; set; } // Decides if the user can add a course to the wishlist
		public int TotalCreditsPicked { get; set; }
		private readonly int MaxCredits; // total number of credits a student can take in a semester

		public IndexModel(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;

			StudentWishlist = new StudentWishlist();
			StudentWishlists = new List<StudentWishlist>();
			StudentWishlistCourse = new List<StudentWishlistCourse>();

			AllowUpsert = false;
			TotalCreditsPicked = 0;
			MaxCredits = 20; //TODO: Maybe change this value to be dynamic
		}

		public IActionResult OnGet(int? wishlistSearchId)
		{
			#region Verify User is logged in 
			var claimsIdentity = (ClaimsIdentity)this.User.Identity;
			var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

			if (claim == null)
			{
				return NotFound();
			}
			#endregion

			#region Fetch Student and Wishlist Information
			var student = _unitOfWork.Student.Get(u => u.UserId == claim.Value);
			StudentWishlists = _unitOfWork.StudentWishlist.GetAll(u => u.StudentId == student.Id, u => u.SemesterInstance.SemesterInstanceStatusId, "SemesterInstance");
			var semesterInstances = _unitOfWork.SemesterInstance.GetAll(u => u.SemesterInstanceStatusId <= 3);
			#endregion

			#region Initialize Semester Wishlist if not Exists
			foreach (var s in semesterInstances)
			{
				bool wishlistExists = false;

				// check if the wishlist already exists
				foreach (var w in StudentWishlists)
				{
					if (w.SemesterInstanceId == s.Id)
					{
						wishlistExists = true;
					}
				}

				// if the wishlist does not exist, then create it
				if (!wishlistExists)
				{
					StudentWishlist = new()
					{
						SemesterInstanceId = s.Id,
						StudentId = student.Id
					};
					_unitOfWork.StudentWishlist.Add(StudentWishlist);
				}
			}

			_unitOfWork.Commit();
			#endregion

			#region Semester Wishlist Drop Down
			StudentWishlists = _unitOfWork.StudentWishlist.GetAll(u => u.StudentId == student.Id, u => u.SemesterInstance.SemesterInstanceStatusId, "SemesterInstance");

			if (wishlistSearchId != null)
			{
				StudentWishlist = _unitOfWork.StudentWishlist.Get(u => u.Id == wishlistSearchId, false, "Student,SemesterInstance");
			}
			else
			{
				if (StudentWishlists.Count() > 0)
				{
					StudentWishlist = StudentWishlists.Last();
				}
			}
			#endregion

			#region find the studentWishlistCourse from the studentWishlist id
			StudentWishlistCourse = _unitOfWork.StudentWishlistCourse.GetAll(u => u.StudentWishlistId == StudentWishlist.Id, null, "Modalities,Campuses,TimeBlocks,Course");

			if (StudentWishlistCourse.Count() > 0)
			{
				foreach (var c in StudentWishlistCourse)
				{
					c.Course.Program = _unitOfWork.AcademicProgram.Get(u => u.Id == c.Course.ProgramId);
					TotalCreditsPicked += c.Course.CreditHours;
				}
			}
			#endregion

			#region Find if there are any courses that can be added to the wishlist with the remaining credits
			int remainingCredits = MaxCredits - TotalCreditsPicked;
			var satisfyingCourses = _unitOfWork.Course.GetAll(u => u.CreditHours <= remainingCredits && u.IsDisabled == false, u => u.ProgramId, "Program");

			// remove courses that are already in the wishlist
			foreach (var c in StudentWishlistCourse)
			{
				satisfyingCourses = satisfyingCourses.Where(u => u.Id != c.CourseId);
			}

			// if there are courses that can be added to the wishlist, then allow upsert
			if (satisfyingCourses.Count() > 0)
			{
				AllowUpsert = true;
			}
			#endregion

			return Page();
		}

		public IActionResult OnPost([FromForm] int wishlistCourseId)
		{
			var selectedCourse = _unitOfWork.StudentWishlistCourse.Get(u => u.Id == wishlistCourseId);

			if (selectedCourse != null)
			{
				_unitOfWork.StudentWishlistCourse.Remove(selectedCourse);
				_unitOfWork.Commit();
			}
			return RedirectToPage("./Index");
		}
	}
}
