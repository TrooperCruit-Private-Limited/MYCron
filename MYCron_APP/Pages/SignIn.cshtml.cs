using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MYCentralModels;
using System.Text.Json;
using System.Text;
using MYCron_APP.DbAccessLite;
using MYCron_APP.DbContextLite;
using MYCentralModels.SQLite;

namespace MYCron_APP.Pages
{
    public class SignInModel(IHttpClientFactory httpClientFactory, List<InternalUrl> internalUrls, SqLiteDbContext dbContext) : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly List<InternalUrl> _internalUrls = internalUrls;
        private readonly SqLiteDbContext _dbContext = dbContext;
        [BindProperty]
        public required SignIn FormModel { get; set; }
        public TransactionStatus TransactionStatus { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var postingJSON = new StringContent(JsonSerializer.Serialize(FormModel), Encoding.UTF8, "application/json");
                using var _httpClient = _httpClientFactory.CreateClient();
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                var response = await _httpClient.PostAsync(CommonAssets.ApiUrl(internalUrls, 0, "SignIn"), postingJSON);
                if (response.IsSuccessStatusCode)
                {
                    var responseJSON = await response.Content.ReadAsStringAsync();
                    AuthenticatedUser authenticatedUser = JsonSerializer.Deserialize<AuthenticatedUser>(responseJSON, CommonAssets.JsonSerializerOptions);
                    if (authenticatedUser != null)
                    {
                        TransactionStatus = authenticatedUser.Status;
                        if (authenticatedUser.Status.Status == 1)
                        {
                            SqLiteDbAccess sqLiteDbAccess = new(_dbContext);
                            await sqLiteDbAccess.SetUserToSession(authenticatedUser.UserSessionData);
                            HttpContext.Session.SetInt32("UserId", authenticatedUser.UserSessionData.Id);
                            HttpContext.Session.SetString("UserName", authenticatedUser.UserSessionData.UserName);
                            HttpContext.Session.SetString("EMail", authenticatedUser.UserSessionData.EMail);
                            HttpContext.Session.SetString("Phone", authenticatedUser.UserSessionData.Phone);
                            return RedirectToPage("./Home");
                        }
                    }
                }
            }
            return Page();
        }
    }
}