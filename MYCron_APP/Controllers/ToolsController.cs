using Microsoft.AspNetCore.Mvc;
using MYCentralModels;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;

namespace MYCron_APP.Controllers
{
    [IgnoreAntiforgeryToken(Order = 1001)]
    public class ToolsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public string FileToBase64String(IFormCollection collection)
        {
            B64SModel model = new();
            if (collection.Files != null && collection.Files.Count > 0)
            {
                model.file = collection.Files.GetFile("attachment");
                if (model.file != null && model.file.Length > 0)
                {
                    model.convertBase64String = Convert.ToBase64String(CommonAssets.GetBytes(model.file));
                }
            }
            return model.convertBase64String;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConvertToWebP()
        {
            var files = Request.Form.Files;

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    using var image = Image.Load(file.OpenReadStream());
                    var fileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}.webp";

                    using var outputStream = new MemoryStream();
                    image.Save(outputStream, new WebpEncoder());
                    outputStream.Position = 0;

                    // Set the appropriate content type for the image
                    var contentType = "image/webp";

                    // Trigger the download by returning a FileResult
                    return File(outputStream.ToArray(), contentType, fileName);
                }
            }
            return Ok();
        }
    }
}
