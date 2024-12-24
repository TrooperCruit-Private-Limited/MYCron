using Microsoft.AspNetCore.Mvc;

namespace MYCron_APP.ViewComponents
{
    public class TitleTextViewComponent : ViewComponent
    {
        [BindProperty]
        public string? TitleClasses { get; set; }
        [BindProperty]
        public string? Suffix { get; set; }
        [BindProperty]
        public string? SuffixClasses { get; set; }
        public async Task<IViewComponentResult> InvokeAsync(string TitleClasses = "title-text", string Suffix = "", string SuffixClasses = "d-none")
        {
            var model = new TitleTextViewComponent
            {
                TitleClasses = TitleClasses,
                Suffix = Suffix,
                SuffixClasses = SuffixClasses
            };
            return await Task.FromResult(View(model));
        }
    }
}
