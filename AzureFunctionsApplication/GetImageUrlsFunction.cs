using CLDV6212_ST10339829_POE;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace AzureFunctionsApp
{
    public class GetImageUrlsFunction
    {
        private readonly AzureBlobService _blobService;

        public GetImageUrlsFunction(AzureBlobService blobService)
        {
            _blobService = blobService;
        }

        [FunctionName("GetImageUrls")]
        public async Task<IActionResult> GetImageUrls(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "get-image-urls")] HttpRequest req, ILogger log)
        {
            log.LogInformation("Fetching image URLs from Blob Storage.");

            try
            {
                // Retrieve image URLs from the AzureBlobService
                var imageUrls = await _blobService.GetFilesAsync();

                // Check if there are any images in the container
                if (imageUrls == null || imageUrls.Count == 0)
                {
                    log.LogInformation("No images found in the blob storage.");
                    return new NotFoundObjectResult("No images found.");
                }

                return new OkObjectResult(imageUrls);
            }
            catch (System.Exception ex)
            {
                log.LogError($"Error fetching image URLs: {ex.Message}");
                return new StatusCodeResult(500); // Internal Server Error
            }
        }
    }
}
