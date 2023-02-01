namespace YouYou.Business.Models
{
    public class Gender : Entity
    {
        public Gender(){}
        public Gender(Guid typeGenderId, string? description)
        {
            TypeGenderId = typeGenderId;
            Description = description;
        }
        public string? Description { get; set; }

        public Guid TypeGenderId { get; set; }

        public TypeGender TypeGender { get; set; }
    }
}