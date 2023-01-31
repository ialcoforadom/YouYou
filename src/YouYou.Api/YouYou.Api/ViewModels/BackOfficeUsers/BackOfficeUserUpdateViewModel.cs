using System.ComponentModel.DataAnnotations;
using YouYou.Api.Extensions;

namespace YouYou.Api.ViewModels.BackOfficeUsers
{
    public class BackOfficeUserUpdateViewModel
    {
        [Key]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(256, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [CPFValidation(ErrorMessage = "O campo {0} está em formato inválido")]
        public string CPF { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
        public string Email { get; set; }

        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "As senhas não conferem.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public Guid RoleId { get; set; }
    }
}
