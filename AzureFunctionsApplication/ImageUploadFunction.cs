using CLDV6212_ST10339829_POE;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace AzureFunctionsApp
{
    public class ImageUploadFunction
    {
        private readonly AzureBlobService _blobService;

        public ImageUploadFunction(AzureBlobService blobService)
        {
            _blobService = blobService;
        }

        // Define the route as '/upload-image' and allow POST requests
        [FunctionName("UploadImage")]
        public async Task<IActionResult> UploadImageToBlob(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "upload-image")] HttpRequest req, ILogger log)
        {
            log.LogInformation("Image upload to Blob Storage function triggered.");

            // Read the form data for the uploaded file
            var formData = await req.ReadFormAsync();
            var file = formData.Files.GetFile("file");

            if (file == null || file.Length == 0)
            {
                log.LogError("No valid image file uploaded.");
                return new BadRequestObjectResult("Please upload a valid image file.");
            }

            try
            {
                // Call the AzureBlobService to handle the upload
                await _blobService.UploadImageAsync(file);
                log.LogInformation($"Image '{file.FileName}' uploaded successfully.");
                return new OkObjectResult($"Image '{file.FileName}' uploaded successfully.");
            }
            catch (System.Exception ex)
            {
                log.LogError($"Error uploading image: {ex.Message}");
                return new StatusCodeResult(500); // Internal Server Error
            }
        }
    }
}
