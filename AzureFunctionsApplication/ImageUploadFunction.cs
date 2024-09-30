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

        [FunctionName("ImageUploadFunction")]
        public async Task<IActionResult> UploadImageToBlob([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Image upload to Blob Storage function triggered.");

            var formData = await req.ReadFormAsync();
            var file = formData.Files.GetFile("file");

            if (file == null || file.Length == 0)
            {
                return new BadRequestObjectResult("Please upload a valid image file.");
            }

            // Call the AzureBlobService to handle the upload
            await _blobService.uploadImageAsync(file);

            return new OkObjectResult($"Image '{file.FileName}' uploaded successfully.");
        }
    }
}
