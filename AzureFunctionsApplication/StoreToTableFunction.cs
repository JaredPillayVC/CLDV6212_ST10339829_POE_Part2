using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos.Table;
using CLDV6212_ST10339829_POE.Models;

namespace AzureFunctionsApp
{
    public static class StoreToTableFunction
    {
        [FunctionName("StoreToTableFunction")]
        public static async Task<IActionResult> StoreToTable([HttpTrigger(AuthorizationLevel.Function, "post", Route = "store/{entityType}")] HttpRequest req,string entityType)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Environment.GetEnvironmentVariable("AzureWebJobsStorage"));
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());

            if (entityType.ToLower() == "customer")
            {
                var data = JsonConvert.DeserializeObject<Customer>(requestBody);
                CloudTable customerTable = tableClient.GetTableReference("Customers");
                await customerTable.CreateIfNotExistsAsync();
                TableOperation insertOperation = TableOperation.Insert(data);
                await customerTable.ExecuteAsync(insertOperation);
                return new OkObjectResult("Customer data stored in Azure Table");
            }
            else if (entityType.ToLower() == "product")
            {
                var data = JsonConvert.DeserializeObject<Product>(requestBody);
                CloudTable productTable = tableClient.GetTableReference("Products");
                await productTable.CreateIfNotExistsAsync();
                TableOperation insertOperation = TableOperation.Insert(data);
                await productTable.ExecuteAsync(insertOperation);
                return new OkObjectResult("Product data stored in Azure Table");
            }
            else
            {
                return new BadRequestObjectResult("Invalid entity type. Use 'customer' or 'product'.");
            }
        }
    }
}
