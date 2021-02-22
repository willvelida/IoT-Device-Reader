using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs.Producer;
using IoTDeviceReader.Models;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using EventDataBatch = Azure.Messaging.EventHubs.Producer.EventDataBatch;

namespace IoTDeviceReader.Functions
{
    public class AssessDeviceDamageLevel
    {
        private readonly ILogger<AssessDeviceDamageLevel> _logger;
        private readonly EventHubProducerClient _highDamageEventHubClient;
        private readonly EventHubProducerClient _lowMediumDamageEventHubClient;

        public AssessDeviceDamageLevel(
            ILogger<AssessDeviceDamageLevel> logger,
            EventHubProducerClient highDamageEventHubClient,
            EventHubProducerClient lowMediumDamageEventHubClient)
        {
            _logger = logger;
            _highDamageEventHubClient = highDamageEventHubClient;
            _lowMediumDamageEventHubClient = lowMediumDamageEventHubClient;
        }

        [FunctionName(nameof(AssessDeviceDamageLevel))]
        public async Task Run([EventHubTrigger("devices", Connection = "EventHubConnectionString")] EventData[] events)
        {
            try
            {
                foreach (EventData eventData in events)
                {
                    string messageBody = Encoding.UTF8.GetString(eventData.Body.Array, eventData.Body.Offset, eventData.Body.Count);

                    var device = JsonConvert.DeserializeObject<Device>(messageBody);

                    if (device.DamageLevel == "High")
                    {
                        // Send to High Damage Event Hub
                        EventDataBatch eventDataBatch = await _highDamageEventHubClient.CreateBatchAsync();
                        var highDamageEvent = JsonConvert.SerializeObject(device);
                        eventDataBatch.TryAdd(new Azure.Messaging.EventHubs.EventData(Encoding.UTF8.GetBytes(highDamageEvent)));
                        await _highDamageEventHubClient.SendAsync(eventDataBatch);
                        _logger.LogInformation($"Device Id: {device.DeviceId} has a high level of damage. Sending to RepairDeviceFunction");
                    }

                    if (device.DamageLevel == "Low" || device.DamageLevel == "Medium")
                    {
                        // Send to LowMedium Event Hub
                        EventDataBatch eventDataBatch = await _lowMediumDamageEventHubClient.CreateBatchAsync();
                        var lowMediumDamageEvent = JsonConvert.SerializeObject(device);
                        eventDataBatch.TryAdd(new Azure.Messaging.EventHubs.EventData(Encoding.UTF8.GetBytes(lowMediumDamageEvent)));
                        await _lowMediumDamageEventHubClient.SendAsync(eventDataBatch);
                        _logger.LogInformation($"Device Id: {device.DeviceId} is in acceptable condition. Sending to DeployDeviceFunction");
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
