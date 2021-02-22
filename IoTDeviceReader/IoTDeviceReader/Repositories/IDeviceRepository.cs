using IoTDeviceReader.Models;
using System.Threading.Tasks;

namespace IoTDeviceReader.Repositories
{
    public interface IDeviceRepository
    {
        /// <summary>
        /// Adds a device to the Device Container.
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        Task AddDevice(Device device);
    }
}
