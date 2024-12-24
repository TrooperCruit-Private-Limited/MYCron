using System.Text.Json;
using Microsoft.AspNetCore.Http;
using MYCentralModels.SQLite;

namespace MYCentralModels
{
    public class CommonAssets
    {
        /// <summary>
        /// Constructs an API URL based on the specified type and URL.
        /// </summary>
        /// <param name="type">The type of API (0-5) to determine the base URL.</param>
        /// <param name="url">The relative URL to append to the base URL.</param>
        /// <returns>The complete API URL.</returns>
        public static string ApiUrl(List<InternalUrl> internalUrls, int type, string url)
        {
            return internalUrls.Where(internalUrl => internalUrl.ApiType == type).FirstOrDefault().ApiUrl + url;
            //return type switch
            //{
            //    0 => "http://localhost/www.TrooperCruit.com.DBI/api/" + url,
            //    1 => "http://localhost/MYControl.DBI/api/" + url,
            //    2 => "http://localhost/MYCheers.DBI/api/" + url,
            //    3 => "http://localhost/Yucta.DBI/api/" + url,
            //    4 => "http://localhost/MYConnect.DBI/api/" + url,
            //    5 => "http://localhost/MYConax.DBI/api/" + url,
            //    _ => url
            //};
        }
        public readonly static int CardsPerSlide = 3;
        public readonly static List<string> pictureallowedContentTypes = ["image/jpeg", "image/png", "image/jpg"];
        public static byte[] GetBytes(IFormFile file)
        {
            using var stream = new MemoryStream(); // Create a MemoryStream object
            file.CopyToAsync(stream); // Copy the file content to the stream
            return stream.ToArray(); // Get the byte array from the stream
        }
        public static JsonSerializerOptions JsonSerializerOptions => new()
        {
            PropertyNameCaseInsensitive = true
        };
    }
    public class B64SModel
    {
        public IFormFile file { get; set; }
        public string convertBase64String { get; set; } = " ";
    }
}