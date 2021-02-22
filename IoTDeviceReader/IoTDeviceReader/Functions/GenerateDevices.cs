using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using IoTDeviceReader.Helpers;
using IoTDeviceReader.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IoTDeviceReader.Functions
{
    public class GenerateDevices
    {
        private readonly ILogger<GenerateDevices> _logger;
        private readonly EventHubProducerClient _eventHubProducerClient;

        public GenerateDevices(
            ILogger<GenerateDevices> logger,
            EventHubProducerClient eventHubProducerClient)
        {
            _logger = logger;
            _eventHubProducerClient = eventHubProducerClient;
        }

        [FunctionName(nameof(GenerateDevices))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "GenerateDevices")] HttpRequest req)
        {
            IActionResult result;

            try
            {
                List<Device> devices = ReadingGenerator.GenerateDevices(1000);
                EventDataBatch eventDataBatch = await _eventHubProducerClient.CreateBatchAsync();

                foreach (var device in devices)
                {
                    var deviceEvent = JsonConvert.SerializeObject(device);
                    eventDataBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(deviceEvent)));
                }

                await _eventHubProducerClient.SendAsync(eventDataBatch);

                result = new StatusCodeResult(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal Server Error. Exception thrown: {ex.Message}");
                result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return result;
        }
    }
}
