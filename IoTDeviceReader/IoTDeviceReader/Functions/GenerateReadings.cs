using Azure.Messaging.ServiceBus;
using IoTDeviceReader.Helpers;
using IoTDeviceReader.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IoTDeviceReader.Functions
{
    public class GenerateReadings
    {
        private readonly ILogger<GenerateReadings> _logger;
        private readonly IConfiguration _config;
        private readonly ServiceBusClient _serviceBusClient;

        private ServiceBusSender _sender;

        public GenerateReadings(
            ILogger<GenerateReadings> logger,
            IConfiguration config,
            ServiceBusClient serviceBusClient)
        {
            _logger = logger;
            _config = config;
            _serviceBusClient = serviceBusClient;

            _sender = serviceBusClient.CreateSender(_config["QueueName"]);
        }

        [FunctionName(nameof(GenerateReadings))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "GenerateReadings")] HttpRequest req)
        {
            IActionResult result;

            try
            {
                // Generate device readings
                List<DeviceReading> deviceReadings = ReadingGenerator.GenerateReadings(100);

                // Send reading to a Service Bus Queue
                foreach (var reading in deviceReadings)
                {
                    var message = new ServiceBusMessage(JsonConvert.SerializeObject(reading));
                    await _sender.SendMessageAsync(message);
                    _logger.LogInformation($"Generating reading {reading.DeviceReadingId}");
                }

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
