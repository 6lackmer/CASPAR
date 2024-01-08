using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Utility;

namespace CASPAR.Pages.Admin
{
    [Authorize(Roles = SD.AdminRole)]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
