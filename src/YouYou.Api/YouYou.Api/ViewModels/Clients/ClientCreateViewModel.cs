using System.ComponentModel.DataAnnotations;
using YouYou.Api.Extensions;
using YouYou.Api.ViewModels.Addresses;
using YouYou.Api.ViewModels.Genders;

namespace YouYou.Api.ViewModels.Clients
{
    public class ClientCreateViewModel
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(256, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [CpfOrCnpjValidation(ErrorMessage = "O campo {0} está em formato inválido")]
        public string CpfOrCnpj { get; set; }

        [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
        public string Email { get; set; }
        public DateTime? Birthday { get; set; }
        public GenderViewModel? Gender { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "As senhas não conferem.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLengthList(13, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 8)]
        public ICollection<string> Phones { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public AddressViewModel Address { get; set; }
        public ICollection<Guid> RolesId { get; set; }
    }
}
