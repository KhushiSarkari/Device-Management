using System.Collections.Generic;
using System.Threading.Tasks;
using dm_backend.EFModels;
using dm_backend.Models;

namespace dm_backend.Data
{
    public interface IDeviceRepository
    {
    
            List<devices> GetAllDevices(string device_name,string serial_number,string status_name,string SortColumn,string SortDirection);
           
            List<devices> GetDeviceById( int device_id);
           string addDevice(devices d);
           string updateDevice(int device_id,devices d);
            int deleteDevice(int device_id);
            string assignDevice(Assign a);
            List<devices> getDeviceDescriptionbyid(int device_id);
            List<Specifications> getAllSpecifications();
            List<Specifications> getSpecificationById(int specification_id);
            string addSpecification(Specification s);
            string updateSpecification(int specification_id,Specification s);
            int deleteSpecification(int specification_id);
            string addType(DeviceType t);
            string addBrand(DeviceBrand b);
            string addModel(DeviceModel m);
           List<devices> getPreviousDevice(int id,string ToSearch, string ToSort,string Todirection);
           List<devices> getCurrentDevice(int id,string  ToSearch, string ToSort,string Todirection);
    }
}