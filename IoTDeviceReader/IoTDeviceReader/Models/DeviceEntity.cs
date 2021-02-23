using Microsoft.Azure.Cosmos.Table;

namespace IoTDeviceReader.Models
{
    public class DeviceEntity : TableEntity
    {
        public DeviceEntity()
        {

        }

        public DeviceEntity(string deviceId, int age)
        {
            PartitionKey = deviceId;
            RowKey = age.ToString();
        }
    }
}
