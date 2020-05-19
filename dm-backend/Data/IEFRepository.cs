using System.Collections.Generic;
using System.Threading.Tasks;
using dm_backend.Models;

namespace dm_backend.Data
{
    public interface IEFRepository
    {
          Task<Statistics> GetStatus();
            List<devices> GetAllDevices();
            List<DeviceInsertUpdate> GetDeviceById( int device_id);
            List<devices> getDeviceDescriptionbyid(int device_id);
            List<Specifications> getAllSpecifications();
            List<Specifications> getSpecificationById(int specification_id);
    }
}