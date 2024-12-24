using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MYCron_APP.DbAccessLite;
using MYCron_APP.DbContextLite;

namespace MYCron_APP.Pages
{
    public class SignOutModel(SqLiteDbContext dbContext) : PageModel
    {
        private readonly SqLiteDbContext _dbContext = dbContext;
        public async Task<IActionResult> OnGet()
        {
            try
            {
                SqLiteDbAccess sqLiteDbAccess = new(_dbContext);
                await sqLiteDbAccess.RemoveUserFromSession(HttpContext.Session.GetInt32("UserId"));
                HttpContext.Session.Remove("UserId");
            }
            catch (Exception ex)
            {
            }
            return RedirectToPage("./Index");
        }
    }
}
