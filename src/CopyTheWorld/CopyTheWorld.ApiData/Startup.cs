using Azure.DigitalTwins.Core;
using Azure.Identity;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(CopyTheWorld.ApiData.Startup))]
namespace CopyTheWorld.ApiData
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var adtEndpoint = builder.GetContext().Configuration["AdtEndpoint"];
            _ = builder.Services.AddSingleton(new DigitalTwinsClient(
                new Uri($"https://{adtEndpoint}"), new DefaultAzureCredential()));
        }
    }
}