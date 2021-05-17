using Azure.Messaging.EventHubs.Producer;
using Azure.Messaging.ServiceBus;
using IoTDeviceReader;
using IoTDeviceReader.Helpers.Table;
using IoTDeviceReader.Models;
using IoTDeviceReader.Repositories;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

[assembly: FunctionsStartup(typeof(Startup))]
namespace IoTDeviceReader
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            builder.Services.AddSingleton<IConfiguration>(config);

            var cosmosClientOptions = new CosmosClientOptions
            {
                ConnectionMode = ConnectionMode.Direct,
            };

            builder.Services.AddSingleton((s) => new CosmosClient(config["CosmosDBConnectionString"], cosmosClientOptions));
            builder.Services.AddSingleton((s) => new ServiceBusClient(config["ServiceBusConnectionString"]));
            builder.Services.AddSingleton((s) => new EventHubProducerClient(config["EventHubConnectionString"], config["EventHubName"]));
            builder.Services.AddSingleton((s) => new EventHubProducerClient(config["EventHubConnectionString"], config["LowMediumDamageEventHubName"]));
            builder.Services.AddSingleton((s) => new EventHubProducerClient(config["EventHubConnectionString"], config["HighDamageEventHubName"]));
            builder.Services.AddSingleton((s) => new CosmosTableHelper<DeviceEntity>(config["StorageConnectionString"], config["TableName"]));

            builder.Services.AddTransient<IDeviceReadingRepository, DeviceReadingRepository>();
            builder.Services.AddTransient<ICoreRepository, CoreRepository>();
            builder.Services.AddTransient<IDeviceRepository, DeviceRepository>();
        }
    }
}
