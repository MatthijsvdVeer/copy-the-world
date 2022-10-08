using Azure.Data.Tables;
using Azure.DigitalTwins.Core;
using Azure.Identity;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(CopyTheWorld.Functions.Startup))]
namespace CopyTheWorld.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var adtEndpoint = builder.GetContext().Configuration["AdtEndpoint"];
            _ = builder.Services.AddSingleton(new DigitalTwinsClient(
                new Uri(adtEndpoint), new DefaultAzureCredential()));

            var tableEndpoint = builder.GetContext().Configuration["TableEndpoint"];
            var tableName = builder.GetContext().Configuration["TableName"];
            _ = builder.Services.AddSingleton(new TableClient(
                $"{tableEndpoint}/{tableName}",
                tableName));
        }
    }
}