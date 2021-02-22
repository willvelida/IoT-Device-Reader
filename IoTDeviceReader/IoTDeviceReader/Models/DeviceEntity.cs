using Microsoft.Azure.Cosmos.Table;

namespace IoTDeviceReader.Models
{
    public class DeviceEntity : TableEntity
    {
        public DeviceEntity()
        {

        }

        public DeviceEntity(string deviceId, string damageLevel)
        {
            PartitionKey = deviceId;
            RowKey = damageLevel;
        }
    }
}
