using IoTDeviceReader.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IoTDeviceReader.Repositories
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly IConfiguration _config;
        private readonly CosmosClient _cosmosClient;

        private Container _container;

        public DeviceRepository(
            IConfiguration config,
            CosmosClient cosmosClient)
        {
            _config = config;
            _cosmosClient = cosmosClient;

            _container = _cosmosClient.GetContainer(_config["DatabaseName"], _config["ContainerName"]);
        }
        public async Task AddReading(DeviceReading deviceReading)
        {
            ItemRequestOptions itemRequestOptions = new ItemRequestOptions
            {
                EnableContentResponseOnWrite = false
            };

            await _container.CreateItemAsync(deviceReading, new PartitionKey(deviceReading.DeviceReadingId), itemRequestOptions);
        }
    }
}
