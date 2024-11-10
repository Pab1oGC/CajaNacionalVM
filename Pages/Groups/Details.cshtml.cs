using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CNSVM.Pages.Groups
{

    [Authorize(Roles = "D")]
    public class DetailsModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
