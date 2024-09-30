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

            // Retrieve image URLs from the AzureBlobService
            var imageUrls = await _blobService.GetFilesAsync();

            return new OkObjectResult(imageUrls);
        }
    }
}
