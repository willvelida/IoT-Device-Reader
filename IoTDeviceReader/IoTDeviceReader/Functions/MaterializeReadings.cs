using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IoTDeviceReader.Models;
using IoTDeviceReader.Repositories;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace IoTDeviceReader.Functions
{
    public class MaterializeReadings
    {
        private readonly ILogger<MaterializeReadings> _logger;
        private readonly ICoreRepository _coreRepository;

        public MaterializeReadings(
            ILogger<MaterializeReadings> logger,
            ICoreRepository coreRepository)
        {
            _logger = logger;
            _coreRepository = coreRepository;
        }

        [FunctionName("MaterializeReadings")]
        public async Task Run([CosmosDBTrigger(
            databaseName: "DeviceDB",
            collectionName: "DeviceReadings",
            ConnectionStringSetting = "CosmosDBConnectionString",
            LeaseCollectionName = "leases",
            CreateLeaseCollectionIfNotExists = true,
            FeedPollDelay = 10)]IReadOnlyList<Document> input)
        {
            try
            {
                if (input != null && input.Count > 0)
                {
                    foreach (var document in input)
                    {
                        var materializedDeviceReading = JsonConvert.DeserializeObject<DeviceReading>(document.ToString());

                        await _coreRepository.AddReading(materializedDeviceReading);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong. Exception thrown: {ex.Message}");
                throw;
            }
        }
    }
}
