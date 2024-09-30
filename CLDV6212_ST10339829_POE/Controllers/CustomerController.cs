using Microsoft.AspNetCore.Mvc;
using CLDV6212_ST10339829_POE.Models;

namespace CLDV6212_ST10339829_POE.Controllers
{
    public class CustomerController : Controller
    {
        private readonly TableService _tableService;
        public CustomerController()
        {
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=st10339829;AccountKey=b1RzjUhuhot2MIrD+6YOgiT2AMeWOX5b5ILd6ROUzt30pD8LVb7GnwPAGKeuP3nPyRX8lGmlwVr2+AStHgokZw==;EndpointSuffix=core.windows.net";
            _tableService = new TableService(connectionString);
        }

        public async Task<IActionResult> Index()
        {
            var customers = await _tableService.GetCustomersAsync();
            return View(customers);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                await _tableService.AddCustomerTableAsync(customer);
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }
    }
}
