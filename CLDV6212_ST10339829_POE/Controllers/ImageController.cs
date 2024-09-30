using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;

namespace CLDV6212_ST10339829_POE.Controllers
{
    public class ImageController : Controller
    {
        private readonly string _connectionString = "DefaultEndpointsProtocol=https;AccountName=st10339829;AccountKey=cOF7hh8IkmDMvijlGOFBy0bchy4PgaO2Rvj4ebBJcCQ2wW2B/lUEgRigBoAn2E8kfyD6jiMsVnNr+AStlo/5LA==;EndpointSuffix=core.windows.net";
        private readonly string _container = "images";
        public IActionResult Index() 
        {
            var files = GetUrl();
            ViewBag.ImageUrls = files;
            return View(files);      
        }

        [HttpPost]
        public async Task<IActionResult> UploadImageIndex(IFormFile formFile) 
        { 
            if (formFile != null && formFile.Length > 0) 
            { 
                var formFileName = Path.GetFileName(formFile.FileName);
                var url = await UploadImageAsync(formFile, formFileName);
                TempData["Message"] = "Your image has been successfully uploaded! ";
            }
            else
            {
                TempData["Error"] = "Please select an image to upload.";
            }
            return RedirectToAction("Index");
        }

        public async Task<string> UploadImageAsync(IFormFile image, string imageName)
        {
            BlobServiceClient blobService = new BlobServiceClient(_connectionString);
            BlobContainerClient blobContainer = blobService.GetBlobContainerClient(_container);

            await blobContainer.CreateIfNotExistsAsync();

            BlobClient blobClient = blobContainer.GetBlobClient(imageName);
            using (var stream = image.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, overwrite: true);
            }

            return blobClient.Uri.ToString();
        }

        public List<string> GetUrl()
        {
            BlobServiceClient blobService = new BlobServiceClient(_connectionString);
            BlobContainerClient blobContainer = blobService.GetBlobContainerClient(_container);
            var url = new List<string>();

            foreach(var blobs in blobContainer.GetBlobs())
            {
                url.Add(blobContainer.GetBlobClient(blobs.Name).Uri.ToString());
            }
            return url;
        }
    }
}
