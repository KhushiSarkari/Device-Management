namespace dm_backend.Data
{
    public class BaseQueryParams
    {
        public int? Id { get; set; }
        public string? Search { get; set; }
        public string? SortField { get; set; }
        public string? SortDirection { get; set; } = "asc";
        public int? PageNo { get; set; }
        public int? PageSize { get; set; }
    }
}