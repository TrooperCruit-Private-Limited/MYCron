using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MYCentralModels;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace MYCron_APP.Pages
{
    public class ConfirmationMessageModel() : PageModel
    {
        public TransactionStatus TransactionStatus { get; set; }
        public void OnGet(string transactionStatus)
        {
            TransactionStatus = JsonSerializer.Deserialize<TransactionStatus>(WebUtility.UrlDecode(transactionStatus), CommonAssets.JsonSerializerOptions);
        }
    }
}
