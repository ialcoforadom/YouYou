using System.ComponentModel.DataAnnotations;

namespace YouYou.Api.ViewModels.Users
{
    public class RoleViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(256, ErrorMessage = "O campo {0} deve ter no máximo {1} caracteres")]
        public string Name { get; set; }
    }
}
