﻿using IoTDeviceReader.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
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

            _container = _cosmosClient.GetContainer(_config["DatabaseName"], _config["DeviceContainer"]);
        }

        public async Task AddDevice(Device device)
        {
            ItemRequestOptions itemRequestOptions = new ItemRequestOptions
            {
                EnableContentResponseOnWrite = false
            };

            await _container.CreateItemAsync(
                device,
                new PartitionKey(device.DeviceId),
                itemRequestOptions);
        }
    }
}
