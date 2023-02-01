using System.ComponentModel.DataAnnotations;

namespace YouYou.Api.ViewModels.Genders
{
    public class GenderViewModel
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public Guid TypeGenderId { get; set; }

        public string Description { get; set; }
    }
}
