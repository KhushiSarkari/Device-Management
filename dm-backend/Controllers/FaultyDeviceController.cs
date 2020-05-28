using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using dm_backend.Models;
using dm_backend.Data;

namespace dm_backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class FaultyDeviceController : ControllerBase
    {
        public IDeviceRepository _repo;
        public FaultyDeviceController(IDeviceRepository repo)
        {
            _repo = repo;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult GetDeviceLisT(string search, string serialNumber, string sortAttribute, string direction)
        {
            string sortDirection = "asc";
            int pageNumber = Convert.ToInt32((string)HttpContext.Request.Query["page"]);
            int pageSize = Convert.ToInt32((string)HttpContext.Request.Query["page-size"]);

            if (!string.IsNullOrEmpty(HttpContext.Request.Query["serial-number"]))
                serialNumber = (HttpContext.Request.Query["serial-number"]);

            if (!string.IsNullOrEmpty(HttpContext.Request.Query["search"]))
                search = HttpContext.Request.Query["search"];

            if (!string.IsNullOrEmpty(HttpContext.Request.Query["sort"]))
                sortAttribute = HttpContext.Request.Query["sort"];

            if (!string.IsNullOrEmpty(HttpContext.Request.Query["direction"]))
                sortDirection = HttpContext.Request.Query["direction"];
            var pager = PagedList<FaultyDeviceModel>.ToPagedList(_repo.getFaultyDevice(search, serialNumber, sortAttribute, sortDirection), pageNumber, pageSize);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(pager.getMetaData()));

            var result = pager;
            return Ok(JsonConvert.SerializeObject(result, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            }));


        }

        [Authorize(Roles = "admin")]
        [HttpPut]
        [Route("resolve")]
        public IActionResult PutResolveRequest([FromBody] FaultyDeviceModel fault)
        {
            string result = null;
            try
            {
                result = _repo.ResolveRequest(fault.complaintId);
            }
            catch (Exception n)
            {
                Console.WriteLine(n.Message);
            }
            return Ok(result);
        }

        [Authorize(Roles = "admin")]
        [HttpPut]
        [Route("markfaulty")]
        public IActionResult PutReportFaultyRequest([FromBody] FaultyDeviceModel faulty)
        {
            string result = null;
            try
            {
                result = _repo.MarkFaultyRequest(faulty.complaintId);
            }
            catch (Exception n)
            {
                Console.WriteLine(n.Message);
            }
            return Ok(result);
        }
    }
}
