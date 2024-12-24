using MYCentralModels.SQLite;

namespace MYCron_APP.Pages.Middlewares
{
    public class SessionHandlerMiddleware(RequestDelegate next, List<OutOfSessionPage> outOfSessionPages)
    {
        private readonly RequestDelegate _next = next;
        private readonly List<OutOfSessionPage> _outOfSessionPages = outOfSessionPages;
        private readonly List<string> ignorePaths =
        [
            "images"
        ];
        public async Task InvokeAsync(HttpContext context)
        {
            string[] pathArray = context.Request.Path.Value.Trim('/').Split('/');
            string folderName = pathArray.FirstOrDefault() ?? "";
            string pathName = pathArray.LastOrDefault() == "" ? "Index" : pathArray.LastOrDefault();

            if (ignorePaths.Contains(folderName) || _outOfSessionPages.Any(page => page.PagePath == pathName))
            {
                await _next(context);
            }
            else if (context.Session is null)
            {
                context.Response.Redirect("./SignOut");
                return;
            }
            else
            {
                await _next(context);
            }
        }
    }
}