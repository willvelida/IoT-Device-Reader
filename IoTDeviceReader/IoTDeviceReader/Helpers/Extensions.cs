using Azure.Messaging.EventHubs.Producer;
using IoTDeviceReader.Models;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

namespace IoTDeviceReader.Helpers
{
    public static class Extensions
    {
        public static async Task SendEvent(this EventHubProducerClient eventHubProducerClient, Device device)
        {
            EventDataBatch eventDataBatch = await eventHubProducerClient.CreateBatchAsync();
            var highDamageEvent = JsonConvert.SerializeObject(device);
            eventDataBatch.TryAdd(new Azure.Messaging.EventHubs.EventData(Encoding.UTF8.GetBytes(highDamageEvent)));
            await eventHubProducerClient.SendAsync(eventDataBatch);
        }
    }
}
