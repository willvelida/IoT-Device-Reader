using IoTDeviceReader.Models;
using IoTDeviceReader.Repositories;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace IoTDeviceReader.Functions
{
    public class DeployDevice
    {
        private readonly ILogger<DeployDevice> _logger;
        private readonly IDeviceRepository _deviceRepository;

        public DeployDevice(
            ILogger<DeployDevice> logger,
            IDeviceRepository deviceRepository)
        {
            _logger = logger;
            _deviceRepository = deviceRepository;
        }

        [FunctionName(nameof(DeployDevice))]
        public async Task Run([EventHubTrigger("lowmediumdamage", Connection = "EventHubConnectionString")] EventData[] events)
        {
            foreach (EventData eventData in events)
            {
                try
                {
                    string messageBody = Encoding.UTF8.GetString(eventData.Body.Array, eventData.Body.Offset, eventData.Body.Count);

                    var device = JsonConvert.DeserializeObject<Device>(messageBody);

                    await _deviceRepository.AddDevice(device);
                    _logger.LogInformation($"Device with ID: {device.DeviceId} is ready for deployment");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Something went wrong. Exception thrown: {ex.Message}");
                    throw;
                }
            }
        }
    }
}
