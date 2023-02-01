namespace YouYou.Business.Models
{
    public class Gender : Entity
    {
        public string Description { get; set; }

        public Guid TypeGenderId { get; set; }

        public TypeGender TypeGender { get; set; }
    }
}