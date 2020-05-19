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
    public interface IDropdownRepository
    {
        IQueryable<GenericDropdownModel> GetAllCountries();
        IQueryable<GenericDropdownModel> GetAllStates(string CountryId);
        IEnumerable<GenericDropdownModel> GetAllCities(string StateId);
        IQueryable<GenericDropdownModel> GetAllDepartments();
        IQueryable<GenericDropdownModel> GetAllCountrycodes();
        IQueryable<GenericDropdownModel> GetAllDesignations(string DepartmentName);
        IQueryable<GenericDropdownModel> GetAllSalutations();
        IQueryable<GenericDropdownModel> GetAllContactType();
        IQueryable<GenericDropdownModel> GetAllAddressType();
        IQueryable<GenericDropdownModel> GetAllDeviceBrands(string type);
        IQueryable<GenericDropdownModel> GetAllDeviceModels(string brand);
        IQueryable<GenericDropdownModel> GetAllDeviceTypes();
        IQueryable<GenericDropdownModel> GetAllStatus();
        List<PartialUserModel> GetAllUserList();
        IQueryable<EFModels.Specification> GetAllSpecifications(string type,string brand,string model);
         

    }
}