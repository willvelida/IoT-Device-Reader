using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using IoTDeviceReader.Models;
using System.Collections.Generic;

namespace IoTDeviceReader.Functions
{
    public static class DeviceBindingsAPI
    {
        [FunctionName("GetDeviceReadingByLocation")]
        public static async Task<IActionResult> GetDeviceReadingByLocation(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Device/{location}")] HttpRequest req,
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
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Device/{id}")] HttpRequest req,
            [CosmosDB(
                databaseName: "DeviceDB",
                collectionName: "DeviceReadings",
                ConnectionStringSetting = "CosmosDBConnectionString",
                Id = "{id}",
                PartitionKey = "{id}")] DeviceReading deviceReading,
            ILogger log)
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
    }
}