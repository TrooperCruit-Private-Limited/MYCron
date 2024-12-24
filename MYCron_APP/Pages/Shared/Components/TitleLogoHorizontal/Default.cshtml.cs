using Microsoft.AspNetCore.Mvc;

namespace MYCron_APP.ViewComponents
{
    public class TitleLogoHorizontalViewComponent : ViewComponent
    {
        [BindProperty]
        public int LogoSize { get; set; }
        [BindProperty]
        public string? TitleClasses { get; set; }
        [BindProperty]
        public string? Suffix { get; set; }
        [BindProperty]
        public string? SuffixClasses { get; set; }
        public async Task<IViewComponentResult> InvokeAsync(int LogoSize = 50, string TitleClasses = "title-text", string Suffix = "", string SuffixClasses = "d-none")
        {
            var model = new TitleLogoVerticalViewComponent
            {
                LogoSize = LogoSize,
                TitleClasses = TitleClasses,
                Suffix = Suffix,
                SuffixClasses = SuffixClasses
            };
            return await Task.FromResult(View(model));
        }
    }
}
