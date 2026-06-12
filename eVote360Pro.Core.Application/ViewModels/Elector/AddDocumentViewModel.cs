using System.ComponentModel.DataAnnotations;

namespace eVote360Pro.Core.Application.ViewModels.Elector
{
    public class AddDocumentViewModel
    {
        [Required(ErrorMessage = "El número de documento es obligatorio.")]
        [StringLength(20, ErrorMessage = "El documento no puede exceder los 20 caracteres.")]
        [DataType(DataType.Text)]
        public required string Document { get; set; }
    }
}