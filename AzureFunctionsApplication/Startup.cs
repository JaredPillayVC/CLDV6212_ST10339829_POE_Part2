using CLDV6212_ST10339829_POE;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(AzureFunctionsApp.Startup))]

namespace AzureFunctionsApp
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // Register AzureBlobService as a singleton service
            string connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            builder.Services.AddSingleton(new AzureBlobService(connectionString));
        }
    }
}
