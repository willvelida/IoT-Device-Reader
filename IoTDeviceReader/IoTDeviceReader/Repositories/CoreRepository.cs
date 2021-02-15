using IoTDeviceReader.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IoTDeviceReader.Repositories
{
    public class CoreRepository : ICoreRepository
    {
        private readonly IConfiguration _configuration;
        private readonly CosmosClient _cosmosClient;

        private Container _container;

        public CoreRepository(
            IConfiguration configuration,
            CosmosClient cosmosClient)
        {
            _configuration = configuration;
            _cosmosClient = cosmosClient;

            _container = _cosmosClient.GetContainer(_configuration["DatabaseName"], _configuration["CoreContainerName"]);
        }

        public async Task AddReading(DeviceReading deviceReading)
        {
            ItemRequestOptions itemRequestOptions = new ItemRequestOptions
            {
                EnableContentResponseOnWrite = false
            };

            await _container.CreateItemAsync(deviceReading, new PartitionKey(deviceReading.DeviceLocation), itemRequestOptions);
        }
    }
}
