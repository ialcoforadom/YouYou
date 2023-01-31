namespace YouYou.Business.Models
{
    public class ExtraPhone : Entity
    {
        public string Number { get; set; }

        public Guid UserId { get; set; }

        public ApplicationUser User { get; set; }

        public ExtraPhone() { }

        public ExtraPhone(string number, Guid userId)
        {
            Number = number;
            UserId = userId;
        }
    }
}
