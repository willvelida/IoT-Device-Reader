using IoTDeviceReader.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace IoTDeviceReader.Functions
{
    public static class DeviceBindingsAPI
    {
        [FunctionName("GetDeviceReadingByLocation")]
        public static async Task<IActionResult> GetDeviceReadingByLocation(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "DeviceReading/{location}")] HttpRequest req,
            [CosmosDB(
                databaseName: "DeviceDB",
                collectionName: "DeviceCore",
                ConnectionStringSetting = "CosmosDBConnectionString",
                SqlQuery = "SELECT * FROM DeviceReadings c WHERE c.location = {location}"
                )] IEnumerable<DeviceReading> deviceReadings,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            return new OkObjectResult(deviceReadings);
        }

        [FunctionName("GetDeviceReadingById")]
        public static async Task<IActionResult> GetDeviceReadingById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "DeviceReading/{id}")] HttpRequest req,
            [CosmosDB(
                databaseName: "DeviceDB",
                collectionName: "DeviceReadings",
                ConnectionStringSetting = "CosmosDBConnectionString",
                Id = "{id}",
                PartitionKey = "{id}")] DeviceReading deviceReading)
        {
            if (deviceReading == null)
            {
                return new NotFoundResult();
            }
            else
            {
                return new OkObjectResult(deviceReading);
            }
        }

        [FunctionName("CreateDeviceReading")]
        public static async Task CreateDeviceReading(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "DeviceReading")] HttpRequest req,
            [CosmosDB(
                databaseName: "DeviceDB",
                collectionName: "DeviceCore",
                ConnectionStringSetting = "CosmosDBConnectionString")] IAsyncCollector<DeviceReading> deviceReading)
        {
            string input = await new StreamReader(req.Body).ReadToEndAsync();

            DeviceReading incomingRequest = JsonConvert.DeserializeObject<DeviceReading>(input);

            var reading = new DeviceReading
            {
                DeviceReadingId = Guid.NewGuid().ToString(),
                DeviceLocation = incomingRequest.DeviceLocation,
                DeviceTempreature = incomingRequest.DeviceTempreature,
                Longitude = incomingRequest.Longitude,
                Latitude = incomingRequest.Latitude,
                Device = new Device
                {
                    AgeInDays = incomingRequest.Device.AgeInDays,
                    DamageLevel = incomingRequest.Device.DamageLevel,
                    Manufacturer = incomingRequest.Device.Manufacturer,
                    Name = incomingRequest.Device.Name,
                    DeviceId = Guid.NewGuid().ToString()
                }
            };

            await deviceReading.AddAsync(reading);
        }
    }
}
