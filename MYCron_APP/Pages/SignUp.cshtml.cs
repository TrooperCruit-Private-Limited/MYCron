using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MYCentralModels;
using System.Net;
using System.Text.Json;
using System.Text;
using MYCentralModels.SQLite;

namespace MYCron_APP.Pages
{
    public class SignUpModel(IHttpClientFactory httpClientFactory, List<InternalUrl> internalUrls) : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly List<InternalUrl> _internalUrls = internalUrls;
        public int signedUpFor = 0;
        [BindProperty]
        public required SignUp FormModel { get; set; }
        public async Task<IActionResult> OnGetAsync(int UpFor = 0)
        {
            signedUpFor = UpFor;
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var postingJSON = new StringContent(JsonSerializer.Serialize(FormModel), Encoding.UTF8, "application/json");
                using var _httpClient = _httpClientFactory.CreateClient();
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                var response = await _httpClient.PostAsync(CommonAssets.ApiUrl(internalUrls, 0, "SignUp"), postingJSON);
                if (response.IsSuccessStatusCode)
                {
                    var responseJSON = await response.Content.ReadAsStringAsync(); // Await the content
                    return RedirectToPage("./ConfirmationMessage", new { transactionStatus = WebUtility.UrlEncode(responseJSON) });
                }
            }
            return Page();
        }
    }
}