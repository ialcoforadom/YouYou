using System.ComponentModel.DataAnnotations;

namespace YouYou.Api.ViewModels.DocumentPhotos
{
    public class DocumentPhotoUpdateViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string DataFiles { get; set; }
    }
}
