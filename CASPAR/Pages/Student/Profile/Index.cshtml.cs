using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Security.Claims;

namespace CASPAR.Pages.Student.Profile
{
	public class IndexModel : PageModel
	{
		private IUnitOfWork _unitOfWork;

		public Infrastructure.Models.Student Student { get; set; }

		public IndexModel(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
			
			Student = new Infrastructure.Models.Student();
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
				Student = _unitOfWork.Student.Get(u => u.UserId == claim.Value, false, "User,Major");
				if (Student == null)
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
