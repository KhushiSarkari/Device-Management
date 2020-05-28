namespace dm_backend.EFModels
{
    public class UserForListDto
    {
        public int UserId { get; set; }
        public int SalutationId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}