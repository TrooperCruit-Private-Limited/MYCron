using Microsoft.AspNetCore.Mvc;

namespace MYCron_APP.ViewComponents
{
    public class ToastMessageBoxViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult(View());
        }
    }
}
