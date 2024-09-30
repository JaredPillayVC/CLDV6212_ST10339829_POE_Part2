using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;

namespace CLDV6212_ST10339829_POE.Controllers
{
    public class ImageController : Controller
    {
        private readonly string _azureFunctionBaseUrl = "https://st10339829-azurefunctionsapplication.azurewebsites.net/";
        private readonly HttpClient _httpClient;

        public ImageController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            var files = await GetImageUrlsAsync();
            ViewBag.ImageUrls = files;
            return View(files);
        }

        [HttpPost]
        public async Task<IActionResult> UploadImageIndex(IFormFile formFile)
        {
            if (formFile != null && formFile.Length > 0)
            {
                var formFileName = Path.GetFileName(formFile.FileName);
                var url = await UploadImageToFunctionAsync(formFile, formFileName);
                TempData["Message"] = "Your image has been successfully uploaded!";
            }
            else
            {
                TempData["Error"] = "Please select an image to upload.";
            }
            return RedirectToAction("Index");
        }

        // Upload image to Azure Function via HTTP request
        public async Task<string> UploadImageToFunctionAsync(IFormFile image, string imageName)
        {
            var uploadUrl = $"{_azureFunctionBaseUrl}UploadImage"; // Ensure absolute URL

            using (var stream = image.OpenReadStream())
            {
                var content = new MultipartFormDataContent();
                var streamContent = new StreamContent(stream);
                content.Add(streamContent, "file", imageName);

                var response = await _httpClient.PostAsync(uploadUrl, content);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
        }

        // Retrieve image URLs from Azure Function via HTTP request
        public async Task<List<string>> GetImageUrlsAsync()
        {
            var getUrl = $"{_azureFunctionBaseUrl}get-image-urls"; // Ensure absolute URL

            var response = await _httpClient.GetAsync(getUrl);
            response.EnsureSuccessStatusCode();

            var jsonResult = await response.Content.ReadAsStringAsync();
            var imageUrls = JsonSerializer.Deserialize<List<string>>(jsonResult);
            return imageUrls ?? new List<string>();
        }
    }
}
