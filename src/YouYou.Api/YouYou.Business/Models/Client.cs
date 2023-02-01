namespace YouYou.Business.Models
{
    public class Client : Entity
    {
        public Guid AddressId { get; set; }

        public Address Address { get; set; }

        public Guid UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
