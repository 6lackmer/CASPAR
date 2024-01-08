using DataAccess;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Utility;

namespace CASPAR.Pages.Tools.ManageInstructors
{
    [Authorize(Roles = SD.AdminRole)]
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public IEnumerable<Infrastructure.Models.Instructor> Instructors { get; set; }

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            Instructors = new List<Infrastructure.Models.Instructor>();
        }


        public IActionResult OnGet()
        {
            Instructors = _unitOfWork.Instructor.GetAll(u => u.ApplicationUser.FirstName != "Null" && u.ApplicationUser.LockoutEnabled == false, null, "ApplicationUser,ProgramAffiliation");

            return Page();
        }
    }
}
