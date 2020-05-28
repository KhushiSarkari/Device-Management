namespace dm_backend.Utilities
{
    public class PaginationHeader
    {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get ; set ; }
        public int TotalItem { get; set ; }
        public int TotalPages { get; set ; }
       
       public PaginationHeader(int currentPage ,int itemPerPage , int totalItems , int totalPages)
       {
           this.CurrentPage = currentPage;
           this.ItemsPerPage = ItemsPerPage;
           this.TotalItem = totalItems;
           this.TotalPages = totalPages;
       }
    
    }
}