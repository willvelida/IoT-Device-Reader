using System;
using System.Threading.Tasks;
using IoTDeviceReader.Models;
using IoTDeviceReader.Repositories;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace IoTDeviceReader.Functions
{
    public class ProcessReadings
    {
        private readonly ILogger<ProcessReadings> _logger;
        private readonly IDeviceRepository _deviceRepository;

        public ProcessReadings(
            ILogger<ProcessReadings> logger,
            IDeviceRepository deviceRepository)
        {
            _logger = logger;
            _deviceRepository = deviceRepository;
        }

        [FunctionName(nameof(ProcessReadings))]
        public async Task Run([ServiceBusTrigger("devicereadings", Connection = "ServiceBusConnectionString")]string myQueueItem)
        {
            try
            {
                // Deserialize the incoming message to DeviceReading
                var deviceReading = JsonConvert.DeserializeObject<DeviceReading>(myQueueItem);

                // Persist the reading into Cosmos DB.
                await _deviceRepository.AddReading(deviceReading);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong. Exception thrown: {ex.Message}");
                throw;
            }
        }
    }
}
