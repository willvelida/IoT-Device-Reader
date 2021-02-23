using IoTDeviceReader.Helpers.Table;
using IoTDeviceReader.Models;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace IoTDeviceReader.Functions
{
    public class RepairDevice
    {
        private readonly ILogger<RepairDevice> _logger;
        private readonly CosmosTableHelper<DeviceEntity> _cosmosTableHelper;

        public RepairDevice(
            ILogger<RepairDevice> logger,
            CosmosTableHelper<DeviceEntity> cosmosTableHelper)
        {
            _logger = logger;
            _cosmosTableHelper = cosmosTableHelper;
        }

        [FunctionName(nameof(RepairDevice))]
        public async Task Run([EventHubTrigger("highdamage", Connection = "EventHubConnectionString")] EventData[] events)
        {
            try
            {
                foreach (EventData eventData in events)
                {
                    string messageBody = Encoding.UTF8.GetString(eventData.Body.Array, eventData.Body.Offset, eventData.Body.Count);

                    var damagedDevice = JsonConvert.DeserializeObject<Device>(messageBody);

                    var deviceEntity = new DeviceEntity(damagedDevice.DeviceId, damagedDevice.AgeInDays);

                    await _cosmosTableHelper.InsertOrMerge(deviceEntity);
                    _logger.LogInformation($"Damaged device {deviceEntity.PartitionKey} is ready for repair");
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
