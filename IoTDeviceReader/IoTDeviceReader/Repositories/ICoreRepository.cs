using IoTDeviceReader.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IoTDeviceReader.Repositories
{
    public interface ICoreRepository
    {
        Task AddReading(DeviceReading deviceReading);
    }
}
