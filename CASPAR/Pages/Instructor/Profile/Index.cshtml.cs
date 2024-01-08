using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Security.Claims;

namespace CASPAR.Pages.Instructor.Profile
{
	public class IndexModel : PageModel
	{
		private IUnitOfWork _unitOfWork;

		public Infrastructure.Models.Instructor Instructor { get; set; }

		public IndexModel(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
			
			Instructor = new Infrastructure.Models.Instructor();
		}

		public IActionResult OnGet()
		{
			#region Verify User is logged in 
			var claimsIdentity = (ClaimsIdentity)this.User.Identity;
			var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

			if (claim == null)
			{
				return NotFound();
			}
			else
			{
				Instructor = _unitOfWork.Instructor.Get(u => u.UserId == claim.Value, false, "ApplicationUser,ProgramAffiliation");
				if (Instructor == null)
				{
					return NotFound();
				}
				else
				{
					return Page();
				}
			}
			#endregion
		}
	}
}
