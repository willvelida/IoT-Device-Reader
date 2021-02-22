using Newtonsoft.Json;

namespace IoTDeviceReader.Models
{
    public class DeviceReading
    {
        [JsonProperty(PropertyName = "id")]
        public string DeviceReadingId { get; set; }
        [JsonProperty(PropertyName = "tempreature")]
        public double DeviceTempreature { get; set; }
        [JsonProperty(PropertyName = "latitude")]
        public double Latitude { get; set; }
        [JsonProperty(PropertyName = "longitude")]
        public double Longitude { get; set; }
        [JsonProperty(PropertyName = "location")]
        public string DeviceLocation { get; set; }
        [JsonProperty(PropertyName = "device")]
        public Device Device { get; set; }
    }
}
