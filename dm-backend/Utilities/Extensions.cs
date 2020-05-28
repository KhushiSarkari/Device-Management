using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace dm_backend.Utilities
{
    public static class Extensions
    {
        
        public static void AddPagination(this HttpResponse response , int currentPage ,int itemsPerPage ,int totalItems , int totalPages)
        {
            var paginationHeader = new PaginationHeader(currentPage, itemsPerPage , totalItems , totalPages);
            var camelCase = new JsonSerializerSettings();
            camelCase.ContractResolver = new CamelCasePropertyNamesContractResolver();
            response.Headers.Add("Pagination" , JsonConvert.SerializeObject(paginationHeader ,camelCase));
            response.Headers.Add("Access-Control-Expose-Headers","Pagination");
        }
    }
}