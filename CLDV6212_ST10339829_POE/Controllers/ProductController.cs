using Microsoft.AspNetCore.Mvc;
using CLDV6212_ST10339829_POE.Models;

namespace CLDV6212_ST10339829_POE.Controllers
{
    public class ProductController : Controller
    {
        private readonly TableService _tableService;
        public ProductController()
        {
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=st10339829;AccountKey=b1RzjUhuhot2MIrD+6YOgiT2AMeWOX5b5ILd6ROUzt30pD8LVb7GnwPAGKeuP3nPyRX8lGmlwVr2+AStHgokZw==;EndpointSuffix=core.windows.net";

            _tableService = new TableService(connectionString);
        }
        public async Task<IActionResult> Index()
        {
            var products = await _tableService.GetProductsAsync();
            return View(products);
        }
        [HttpGet]
        public IActionResult Create() => View();
        [HttpPost]
        public async Task<IActionResult> Create(Product product) 
        {
           if(ModelState.IsValid)
           {
               await _tableService.AddProductTableAsync(product);
               return RedirectToAction(nameof(Index));
           }
           return View(product);
        }
    }
}
