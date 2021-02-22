using IoTDeviceReader.Models;
using System.Threading.Tasks;

namespace IoTDeviceReader.Repositories
{
    public interface ICoreRepository
    {
        /// <summary>
        /// Adds a device reading to the Core container.
        /// </summary>
        /// <param name="deviceReading"></param>
        /// <returns></returns>
        Task AddReading(DeviceReading deviceReading);
    }
}
