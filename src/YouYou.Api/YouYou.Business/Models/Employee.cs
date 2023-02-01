namespace YouYou.Business.Models
{
    public class Employee : Entity
    {
        public DateTime? Birthday { get; set; }

        public Guid UserId { get; set; }

        public ApplicationUser User { get; set; }

        public Guid AddressId { get; set; }

        public Address Address { get; set; }

        public Guid? BankDataId { get; set; }

        public BankData BankData { get; set; }

        public Guid GenderId { get; set; }

        public Gender Gender { get; set; }

        public ICollection<DocumentPhoto> DocumentPhotos { get; set; }
    }
}