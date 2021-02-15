using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

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
        [JsonProperty(PropertyName = "damageLevel")]
        public string DamageLevel { get; set; }
        [JsonProperty(PropertyName = "location")]
        public string DeviceLocation { get; set; }
    }
}
