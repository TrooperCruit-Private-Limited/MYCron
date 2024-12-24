using MYCentralModels;
using System.Text.Json;
using System.Text;
using MYCentralModels.SQLite;

namespace MYCron_APP.Pages.Middlewares
{
    public class VisitorTrackingMiddleware(IHttpClientFactory httpClientFactory, List<InternalUrl> internalUrls) : IMiddleware
    {
        private readonly List<string> ignorePaths =
        [
            "/images/"
        ];
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly List<InternalUrl> _internalUrls = internalUrls;
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var pathArray = context.Request.Path.Value.Trim('/').Split('/');
            var ipAddress = context.Connection.RemoteIpAddress?.ToString();
            string folderName = "";
            if (pathArray.Length > 1)
            {
                folderName = pathArray[0];
            }
            if (!ignorePaths.Contains(folderName) && ipAddress != "::1")
            {
                // Capture visitor information
                var visitedOn = DateTime.UtcNow;
                var pageName = context.Request.Path; // You can customize this based on your routing

                // Log or store the visitor information (e.g., save to a database)
                SiteVisits siteVisits = new()
                {
                    VisitorIP = ipAddress,
                    VisitedOn = visitedOn,
                    PageName = pageName == "" ? "/Index" : pageName
                };
                var postingJSON = new StringContent(JsonSerializer.Serialize(siteVisits), Encoding.UTF8, "application/json");
                using var _httpClient = _httpClientFactory.CreateClient();
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var response = await _httpClient.PostAsync(CommonAssets.ApiUrl(internalUrls, 0, "LogVisitorDetails"), postingJSON);
                // Call the next middleware in the pipeline
            }
            await next(context);
        }
    }
}