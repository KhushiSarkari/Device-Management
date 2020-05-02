using Microsoft.AspNetCore.Mvc;
using dm_backend.Logics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace dm_backend.Models
{
    [Authorize(Roles="admin")]
    [Route("[controller]")]
    [ApiController]
    public class SortingController : Controller
    {
        public AppDb Db { get; }
        public SortingController(AppDb db)
        {
            Db = db;
        }
        [HttpGet]
        async public Task<IActionResult> Sorting()
        {
            await Db.Connection.OpenAsync();
            string status = HttpContext.Request.Query["status"];
            string sort = HttpContext.Request.Query["sort"];
            string find = HttpContext.Request.Query["user-name"];
            string deviceserialNumber = HttpContext.Request.Query["serial-number"];
            string sortType = HttpContext.Request.Query["sort-type"];
            int page = 1;
            int limit = 5;

            if (!string.IsNullOrEmpty(HttpContext.Request.Query["page-size"]))
                limit = int.Parse(HttpContext.Request.Query["page-size"]);
            if (!string.IsNullOrEmpty(HttpContext.Request.Query["page"]))
                page = int.Parse(HttpContext.Request.Query["page"]);
            if (status == "" || status == null)
                status = null;
            if (deviceserialNumber == "" || deviceserialNumber == null)
                deviceserialNumber = null;
            if (sortType == null)
                sortType = null;
            if (sort == null)
                sort = "";
            if (find == null)
                find = "";
          
          
            var result = new SortRequestHistoryData(Db);
            try
            {
                var pager = PagedList<RequestDeviceHistory>.ToPagedList(await result.GetSortData(find, deviceserialNumber, status, sort, sortType), page, limit);
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(pager.getMetaData()));
                //  return new OkObjectResult(await result.GetSortData(find, deviceserialNumber, status, sort, sortType, page, (limit)));
                return new OkObjectResult( pager);
            }
            catch
            {
                return BadRequest();
            }

        }
    }
}