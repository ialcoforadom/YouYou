using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using YouYou.Api.Extensions;
using YouYou.Api.ViewModels.Addresses;
using YouYou.Api.ViewModels.BankData;
using YouYou.Api.ViewModels.DocumentPhotos;
using YouYou.Api.ViewModels.Genders;

namespace YouYou.Api.ViewModels.Employees
{
    public class EmployeeUpdateViewModel
    {
        [Key]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(256, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Name { get; set; }

        public string NickName { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [CPFValidation(ErrorMessage = "O campo {0} está em formato inválido")]
        public string CPF { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public DateTime Birthday { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLengthList(13, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 8)]
        public ICollection<string> Phones { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public AddressViewModel Address { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public GenderViewModel Gender { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public BankDataViewModel BankData { get; set; }

        public string Email { get; set; }

        public string? Password { get; set; }

        public string? ConfirmPassword { get; set; }

        public ICollection<DocumentPhotoUpdateViewModel> DocumentPhotos { get; set; }

    }
}
