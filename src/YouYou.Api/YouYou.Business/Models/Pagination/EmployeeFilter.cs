namespace YouYou.Business.Models.Pagination
{
    public class EmployeeFilter : PaginationFilterBase
    {
        public string Name { get; set; }

        public string NickName { get; set; }

        public string Plate { get; set; }

        public string City { get; set; }

        public EmployeeFilter() : base()
        {
        }
    }
}
