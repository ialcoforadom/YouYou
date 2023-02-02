namespace YouYou.Business.Models
{
    public class Employee : Entity
    {

        public Guid UserId { get; set; }

        public ApplicationUser User { get; set; }

        public Guid AddressId { get; set; }

        public Address Address { get; set; }

        public Guid? BankDataId { get; set; }

        public BankData BankData { get; set; }
    }
}