using IoTDeviceReader.Models;
using System.Threading.Tasks;

namespace IoTDeviceReader.Repositories
{
    public interface IDeviceReadingRepository
    {
        /// <summary>
        /// Adds a device reading to the device reading container.
        /// </summary>
        /// <param name="deviceReading"></param>
        /// <returns></returns>
        Task AddReading(DeviceReading deviceReading);
    }
}
