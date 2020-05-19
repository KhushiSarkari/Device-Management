using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dm_backend.Controllers;
using dm_backend.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using dm_backend.EFModels;

namespace dm_backend.Data
{
    public class DropdownRepository : IDropdownRepository
    {
    
        private readonly EFDbContext _context;
        public DropdownRepository(EFDbContext context)
        {
            _context = context;
        }

        public IQueryable<GenericDropdownModel> GetAllCountrycodes()
        {
            var countryCodes = from c in _context.Country
            select new GenericDropdownModel()
            {
                Id =c.CountryId,
                Value =Convert.ToString(c.CountryCode)
            };                           
            return countryCodes;
        
        }

        public IQueryable<EFModels.Specification> GetAllSpecifications(string type, string brand, string model)
        {
            var Specifications = from sp in _context.Specification
            join d in _context.Device on sp.SpecificationId equals d.SpecificationId
            join dt in _context.DeviceType on d.DeviceTypeId equals dt.DeviceTypeId
            join db in _context.DeviceBrand on d.DeviceBrandId equals db.DeviceBrandId
            join dm in _context.DeviceModel on d.DeviceModelId equals dm.DeviceModelId
            join ss in _context.Status on d.StatusId equals ss.StatusId
            where db.Brand==brand && dt.Type==type && dm.Model==model && ss.StatusName!="Faulty"
            select sp;
            return Specifications;
           
        }


        public List<PartialUserModel> GetAllUserList()
        {
            var userList = (from us in _context.User
           join dd in _context.DepartmentDesignation on us.DepartmentDesignationId equals dd.DepartmentDesignationId
           join d in _context.Department on dd.DepartmentId equals d.DepartmentId
           join s in _context.Status on us.Status equals s.StatusId
           where s.StatusName=="Active"
           select new PartialUserModel() {
               UserId = us.UserId,
               FirstName =us.FirstName,
               DepartmentName = d.DepartmentName
           }).ToList();
           return userList;
        }

        IQueryable<GenericDropdownModel> IDropdownRepository.GetAllAddressType()
        {
           var AddressTypes = from ad in _context.AddressType
            select new GenericDropdownModel()
            {
                Id =ad.AddressTypeId,
                Value =ad.AddressTypes
            };  
            return AddressTypes;
        }

        IEnumerable<GenericDropdownModel> IDropdownRepository.GetAllCities(string StateId)
        {
           var cities = (from c in _context.City
            where c.StateId == Convert.ToInt16(StateId) || StateId == null
             select new GenericDropdownModel()
            {
                Id =c.CityId,
                Value =c.CityName
             });                    
            return cities.Take(5000);
        }

        IQueryable<GenericDropdownModel> IDropdownRepository.GetAllContactType()
        {
             var ContactTypes = from ct in _context.ContactType
            select new GenericDropdownModel()
            {
                Id =ct.ContactTypeId,
                Value =ct.ContactTypes
            };    
            return ContactTypes;
        }

        IQueryable<GenericDropdownModel> IDropdownRepository.GetAllCountries()
        {
            var countries = from c in _context.Country
            select new GenericDropdownModel()
            {
                Id =c.CountryId,
                Value =c.CountryName
            };                           
            return countries;
        }

        IQueryable<GenericDropdownModel> IDropdownRepository.GetAllDepartments()
        {
           var departments= from dd in _context.Department
            select new GenericDropdownModel()
            {
                Id =dd.DepartmentId,
                Value =dd.DepartmentName
            };      
            return departments;
        }

        IQueryable<GenericDropdownModel> IDropdownRepository.GetAllDesignations(string DepartmentName)
        {
            var designations=(from de in _context.Designation
            join dd in _context.DepartmentDesignation on de.DesignationId equals dd.DesignationId
            join d in _context.Department on dd.DepartmentId equals d.DepartmentId
            where d.DepartmentName==DepartmentName || DepartmentName==null
            select new GenericDropdownModel()
            {
                Id =de.DesignationId,
                Value =de.DesignationName
            }).Distinct();  
            return designations;   
        }

        IQueryable<GenericDropdownModel> IDropdownRepository.GetAllDeviceBrands(string type)
        {
            var brands = (from b in _context.DeviceBrand
            join db in _context.Device on b.DeviceBrandId equals db.DeviceBrandId
            join dt in _context.DeviceType on db.DeviceTypeId equals dt.DeviceTypeId
            where dt.Type==type || type==null
             select new GenericDropdownModel()
            {
                Id =b.DeviceBrandId,
                Value =b.Brand
            }).Distinct();  
            return brands;
        }

        IQueryable<GenericDropdownModel> IDropdownRepository.GetAllDeviceModels(string brand)
        {
            var models = (from m in _context.DeviceModel
            join dm in _context.Device on m.DeviceModelId equals dm.DeviceModelId
            join db in _context.DeviceBrand on dm.DeviceBrandId equals db.DeviceBrandId
            where db.Brand==brand || brand==null
            select new GenericDropdownModel()
            {
                Id =m.DeviceModelId,
                Value =m.Model
            }).Distinct();  
            return models;
        }

        IQueryable<GenericDropdownModel> IDropdownRepository.GetAllDeviceTypes()
        {
            var types =from t in _context.DeviceType
            select new GenericDropdownModel()
            {
                Id =t.DeviceTypeId,
                Value =t.Type
            };  
            return types;
        }

        IQueryable<GenericDropdownModel> IDropdownRepository.GetAllSalutations()
        {
            var salutations = from s in _context.Salutation
            select new GenericDropdownModel()
            {
                Id =s.SalutationId,
                Value =s.SalutationName
            };  
            return salutations;
        }

        IQueryable<GenericDropdownModel> IDropdownRepository.GetAllStates(string CountryId)
        {
            var states = from s in _context.State
            where s.CountryId == Convert.ToInt16(CountryId) || CountryId == null
            select new GenericDropdownModel()
            {
                Id =s.StateId,
                Value =s.StateName
            };                      
            return states;
        }

        IQueryable<GenericDropdownModel> IDropdownRepository.GetAllStatus()
        {
             var status =from st in _context.Status
            where st.StatusName=="Allocated" || st.StatusName=="Free" || st.StatusName=="Faulty"
            select new GenericDropdownModel()
            {
                Id =st.StatusId,
                Value =st.StatusName
            };      
            return status;
        }
    }
}