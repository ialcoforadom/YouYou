namespace YouYou.Api.Models
{
    public class Usuario : Entity
    {
        public string? Name{ get; set; }
        public string? Cpf{ get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public bool IsActived { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
