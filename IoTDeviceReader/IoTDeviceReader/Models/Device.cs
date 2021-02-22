using Newtonsoft.Json;

namespace IoTDeviceReader.Models
{
    public class Device
    {
        [JsonProperty(PropertyName = "id")]
        public string DeviceId { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "manufacturer")]
        public string Manufacturer { get; set; }
        [JsonProperty(PropertyName = "age")]
        public int AgeInDays { get; set; }
        [JsonProperty(PropertyName = "damageLevel")]
        public string DamageLevel { get; set; }
    }
}
