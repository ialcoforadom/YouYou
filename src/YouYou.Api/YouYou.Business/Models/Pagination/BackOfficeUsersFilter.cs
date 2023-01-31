namespace YouYou.Business.Models.Pagination
{
    public class BackOfficeUsersFilter : PaginationFilterBase
    {
        public string Name { get; set; }

        public string CPF { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }

        public BackOfficeUsersFilter() : base()
        {
        }
    }
}
