using Microsoft.AspNetCore.Mvc;

namespace CLDV6212_ST10339829_POE.Controllers
{
    public class OrderController : Controller
    {
        private readonly AzureQueueService _azureQueueService;

        public OrderController()
        {
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=st10339829;AccountKey=b1RzjUhuhot2MIrD+6YOgiT2AMeWOX5b5ILd6ROUzt30pD8LVb7GnwPAGKeuP3nPyRX8lGmlwVr2+AStHgokZw==;EndpointSuffix=core.windows.net";
            _azureQueueService = new AzureQueueService(connectionString);
        }
        public async Task<IActionResult> Index()
        {
            var queuedMessages = await _azureQueueService.RetriveMessagesAsync();
            return View(queuedMessages);
        }
        [HttpPost]
        public async Task<IActionResult> ProcessOrder(string order) 
        {
            if (!string.IsNullOrEmpty(order)) 
            { 
            await _azureQueueService.CreateMessageAsync(order);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
