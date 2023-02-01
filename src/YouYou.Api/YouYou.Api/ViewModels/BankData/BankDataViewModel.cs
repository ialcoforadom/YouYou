using System.ComponentModel.DataAnnotations;
using YouYou.Api.Extensions;

namespace YouYou.Api.ViewModels.BankData
{
    public class BankDataViewModel
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(256, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string BankName { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(5, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 4)]
        public string Agency { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(10, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 4)]
        public string Account { get; set; }

        [CpfOrCnpjValidation(ErrorMessage = "O campo {0} está em formato inválido")]
        public string CpfOrCnpjHolder { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public bool IsHolder { get; set; }

        [StringLength(32, ErrorMessage = "O campo {0} deve ter no máximo {1} caracteres")]
        public string PixKey { get; set; }
    }
}
