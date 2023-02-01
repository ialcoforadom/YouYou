using System.ComponentModel.DataAnnotations;

namespace YouYou.Api.ViewModels.DocumentPhotos
{
    public class DocumentPhotoCreateViewModel
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string DataFiles { get; set; }
    }
}
