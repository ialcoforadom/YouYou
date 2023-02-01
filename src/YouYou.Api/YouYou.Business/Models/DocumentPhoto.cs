namespace YouYou.Business.Models
{
    public class DocumentPhoto : Entity
    {
        public string Name { get; set; }

        public string FileType { get; set; }

        public byte[] DataFiles { get; set; }

        public Guid EmployeeId { get; set; }

        public Employee Employee { get; set; }
    }
}