using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Utility;

namespace CASPAR.Pages.Instructor.Wishlist
{
	[Authorize(Roles = SD.InstructorRole)]
	public class IndexModel : PageModel
	{
		private readonly IUnitOfWork _unitOfWork;
		public InstructorWishlist InstructorWishlist { get; set; }
		public IEnumerable<InstructorWishlist> InstructorWishlists { get; set; }
		public IEnumerable<InstructorWishlistCourse> InstructorWishlistCourse { get; set; }

		public bool AllowUpsert { get; set; } // Decides if the user can add a course to the wishlist
		public int TotalCreditsPicked { get; set; }
		private readonly int MaxCredits; // total number of credits a instructor can take in a semester

		public IndexModel(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;

			InstructorWishlist = new InstructorWishlist();
			InstructorWishlists = new List<InstructorWishlist>();
			InstructorWishlistCourse = new List<InstructorWishlistCourse>();

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

			#region Fetch Instructor and Wishlist Information
			var instructor = _unitOfWork.Instructor.Get(u => u.UserId == claim.Value);
			InstructorWishlists = _unitOfWork.InstructorWishlist.GetAll(u => u.InstructorId == instructor.Id, u => u.SemesterInstance.SemesterInstanceStatusId, "SemesterInstance");
			var semesterInstances = _unitOfWork.SemesterInstance.GetAll(u => u.SemesterInstanceStatusId <= 3);
			#endregion

			#region Initialize Semester Wishlist if not Exists
			foreach (var s in semesterInstances)
			{
				bool wishlistExists = false;

				// check if the wishlist already exists
				foreach (var w in InstructorWishlists)
				{
					if (w.SemesterInstanceId == s.Id)
					{
						wishlistExists = true;
					}
				}

				// if the wishlist does not exist, then create it
				if (!wishlistExists)
				{
					InstructorWishlist = new()
					{
						SemesterInstanceId = s.Id,
						InstructorId = instructor.Id
					};
					_unitOfWork.InstructorWishlist.Add(InstructorWishlist);
				}
			}
			_unitOfWork.Commit();
			#endregion

			#region Semester Wishlist Drop Down
			InstructorWishlists = _unitOfWork.InstructorWishlist.GetAll(u => u.InstructorId == instructor.Id, u => u.SemesterInstance.SemesterInstanceStatusId, "SemesterInstance");


			if (wishlistSearchId != null)
			{
				InstructorWishlist = _unitOfWork.InstructorWishlist.Get(u => u.Id == wishlistSearchId, false, "Instructor,SemesterInstance");
			}
			else
			{
				// Pull latest Instructor Wishlist where the Semester is still pending
				if (InstructorWishlists.Count() > 0)
				{
					InstructorWishlist = InstructorWishlists.Last();
				}
			}
			#endregion

			#region find the instructorWishlistCourse from the instructorWishlist id
			InstructorWishlistCourse = _unitOfWork.InstructorWishlistCourse.GetAll(u => u.InstructorWishlistId == InstructorWishlist.Id, u => u.Ranking, "Modalities,Campuses,TimeBlocks,Course");

			if (InstructorWishlistCourse.Count() > 0)
			{
				foreach (var c in InstructorWishlistCourse)
				{
					c.Course.Program = _unitOfWork.AcademicProgram.Get(u => u.Id == c.Course.ProgramId);
					TotalCreditsPicked += c.Course.CreditHours;
				}
			}
			#endregion

			#region Find if there are any courses that can be added to the wishlist with the remaining credits
			// Find Add the number of release credits to the TotalCreditsPicked
			foreach (var el in _unitOfWork.ReleaseTime.GetAll(u => u.InstructorId == instructor.Id && u.SemesterInstanceId == InstructorWishlist.SemesterInstanceId))
			{
				// TODO: maybe swap this out to a different variable if we want to differentiate between credits picked from courses and credits from release times. 
				TotalCreditsPicked += el.ReleaseTimeAmount;
			}

			int remainingCredits = MaxCredits - TotalCreditsPicked;
			var satisfyingCourses = _unitOfWork.Course.GetAll(u => u.CreditHours <= remainingCredits && u.IsDisabled == false, u => u.ProgramId, "Program");

			foreach (var c in InstructorWishlistCourse)
			{
				satisfyingCourses = satisfyingCourses.Where(u => u.Id != c.CourseId);
			}

			if (satisfyingCourses.Count() > 0)
			{
				AllowUpsert = true;
			}
			#endregion

			return Page();
		}

		public IActionResult OnPostChangeCourseRanking([FromForm] string direction, [FromForm] int wishlistCourseId)
		{
			var selectedIndex = _unitOfWork.InstructorWishlistCourse.Get(u => u.Id == wishlistCourseId);
			var swappedIndex = new InstructorWishlistCourse();

			if (selectedIndex != null)
			{
				if (direction == "UP")
				{
					swappedIndex = _unitOfWork.InstructorWishlistCourse.Get(u =>
						u.InstructorWishlistId == selectedIndex.InstructorWishlistId && u.Ranking == selectedIndex.Ranking - 1
					);

					if (swappedIndex != null)
					{
						swappedIndex.Ranking += 1;
						selectedIndex.Ranking -= 1;
					}
				}
				else if (direction == "DOWN")
				{
					swappedIndex = _unitOfWork.InstructorWishlistCourse.Get(u =>
						u.InstructorWishlistId == selectedIndex.InstructorWishlistId && u.Ranking == selectedIndex.Ranking + 1
					);

					if (swappedIndex != null)
					{
						swappedIndex.Ranking -= 1;
						selectedIndex.Ranking += 1;
					}
				}

				if (swappedIndex != null)
				{
					_unitOfWork.InstructorWishlistCourse.Update(selectedIndex);
					_unitOfWork.InstructorWishlistCourse.Update(swappedIndex);
					_unitOfWork.Commit();
				}
			}

			return RedirectToPage("./Index", new { wishlistSearchId = selectedIndex.InstructorWishlistId.ToString() });
		}

		public IActionResult OnPostDeleteCourse([FromForm] int wishlistCourseId)
		{
			var selectedCourse = _unitOfWork.InstructorWishlistCourse.Get(u => u.Id == wishlistCourseId);

			if (selectedCourse != null)
			{
				_unitOfWork.InstructorWishlistCourse.Remove(selectedCourse);

				// Update the ranking of the courses after the deleted course
				InstructorWishlistCourse = _unitOfWork.InstructorWishlistCourse.GetAll(
				   u => u.InstructorWishlistId == selectedCourse.InstructorWishlistId && u.Ranking > selectedCourse.Ranking,
				   u => u.Ranking
			   );

				foreach (var course in InstructorWishlistCourse)
				{
					course.Ranking -= 1;
					_unitOfWork.InstructorWishlistCourse.Update(course);
				}
				_unitOfWork.Commit();
			}

			return RedirectToPage("./Index", new { wishlistSearchId = selectedCourse.InstructorWishlistId.ToString() });
		}
	}
}
