using Microsoft.AspNetCore.Mvc;

namespace CLDV6212_ST10339829_POE.Controllers
{
    public class ContractsController : Controller
    {
        private readonly AzureFileService _azureFileService;

        public ContractsController()
        {
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=st10339829;AccountKey=b1RzjUhuhot2MIrD+6YOgiT2AMeWOX5b5ILd6ROUzt30pD8LVb7GnwPAGKeuP3nPyRX8lGmlwVr2+AStHgokZw==;EndpointSuffix=core.windows.net";
            _azureFileService = new AzureFileService(connectionString);
        }
        public async Task<IActionResult> Index()
        {
            var files = await _azureFileService.FilesAsync();
            return View(files);
        }
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile formFile)
        {
            if (formFile != null)
            {
                await _azureFileService.UploadAsync(formFile);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
