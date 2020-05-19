using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using dm_backend.Models;
using Microsoft.AspNetCore.Authorization;
using dm_backend.Data;
using Newtonsoft.Json;
using System.Linq;

namespace dm_backend.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class DropdownController : ControllerBase
    {
        public IDropdownRepository _repo;
        public DropdownController( IDropdownRepository repo)
        {
            _repo = repo;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("country")]
        public IActionResult Countries()
        {
            var countries = _repo.GetAllCountries();
            if (countries.Count() > 0)
                return Ok(countries);
            else
                return NoContent();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("state")]
        public IActionResult States()
        {
            String fields = HttpContext.Request.Query["id"];
            var states = _repo.GetAllStates(fields);
            if (states.Count() > 0)
            {
                return Ok(states);
            }
            else
                return NoContent();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("city")]
        public IActionResult Cities()
        {
            String fields = HttpContext.Request.Query["id"];
            var cities = _repo.GetAllCities(fields);
            if (cities.Count() > 0)
            {
                return Ok(cities);
            }
            else
                return NoContent();
        }

        [HttpGet]
        [Route("department")]
        public IActionResult departmentTypes()
        {
            var departments = _repo.GetAllDepartments();
            if (departments.Count() > 0)
            {
                return Ok(departments);
            }
            else
                return NoContent();
        }



        [HttpGet]
        [Route("designation")]
        public IActionResult designationTypes()
        {
            String fields = HttpContext.Request.Query["id"];
            var designations = _repo.GetAllDesignations(fields);
            if (designations.Count() > 0)
            {
                return Ok(designations);
            }
            else
                return NoContent();
        }

        [Route("salutation")]
        public IActionResult Salutations()
        {
            var result = _repo.GetAllSalutations();
            if (result.Count() < 1)
                return NoContent();
            return Ok(result);
        }

        [HttpGet]
        [Route("country_code")]
        public IActionResult CountryCodes()
        {
            var result = _repo.GetAllCountrycodes();
            if (result.Count() > 0)
            {
                return Ok(result);
            }
            else
                return NoContent();
        }

        [HttpGet]
        [Route("addressType")]
        public IActionResult addressTypes()
        {
            var result = _repo.GetAllAddressType();
            if (result.Count() < 1)
                return NoContent();
            return Ok(result);
        }

        [HttpGet]
        [Route("contactType")]
        public IActionResult contactTypes()
        {
            var result = _repo.GetAllContactType();
            if (result.Count() < 1)
                return NoContent();
            return Ok(result);
        }

        [HttpGet]
        [Route("{type}/{brand}/{model}/specification")]
        public IActionResult GetAllSpecification(string type, string model, string brand)
        {
            var result = _repo.GetAllSpecifications(type, brand, model);
            if (result.Count() < 1)
                return NoContent();
            return Ok(result);
        }

        [HttpGet]
        [Route("brands")]
        public IActionResult GetAllDeviceBrands()
        {
            String fields = HttpContext.Request.Query["type"];
            var result = _repo.GetAllDeviceBrands(fields);
            if (result.Count() < 1)
                return NoContent();
            return Ok(result);
        }

        [HttpGet]
        [Route("models")]
        public IActionResult GetAllDeviceModels()
        {
            String fields = HttpContext.Request.Query["brand"];
            var result = _repo.GetAllDeviceModels(fields);
            if (result.Count() < 1)
                return NoContent();
            return Ok(result);
        }

        [HttpGet]
        [Route("types")]
        public IActionResult GetAllDeviceTypes()
        {
            var result = _repo.GetAllDeviceTypes();
            if (result.Count() < 1)
                return NoContent();
            return Ok(result);
        }

        [HttpGet("status")]
        public IActionResult GetAllstatus()
        {
            var result = _repo.GetAllStatus();
            if (result.Count() < 1)
                return NoContent();
            return Ok(result);
        }

        [HttpGet]
        [Route("userlist")]
        public IActionResult GetUserDetails()
        {
            var result = _repo.GetAllUserList();
            if (result.Count > 0)
                return Ok(result);
            else
                return NoContent();
        }
    } 
}