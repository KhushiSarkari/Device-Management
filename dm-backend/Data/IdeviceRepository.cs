using System.Collections.Generic;
using System.Threading.Tasks;
using dm_backend.EFModels;
using dm_backend.Models;

namespace dm_backend.Data
{
    public interface IDeviceRepository
    {
    
            List<devices> GetAllDevices(string device_name,string serial_number,string status_name,string SortColumn,string SortDirection);
            List<DeviceInsertUpdate> GetDeviceById( int device_id);
            List<devices> getDeviceDescriptionbyid(int device_id);
            List<Specifications> getAllSpecifications();
            List<Specifications> getSpecificationById(int specification_id);
            string addSpecification(Specification s);
            string updateSpecification(int specification_id,Specification s);
            Task<Specification> deleteSpecification(int specification_id);
    }
}