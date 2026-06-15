using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace eVote360Pro.Core.Application.ViewModels.Elector
{
    public class LoadCedulaViewModel
    {
        public required string Document { get; set; }

        [Required(ErrorMessage = "Debe subir una imagen de su cédula para validar su identidad.")]
        public IFormFile? CedulaImage { get; set; }
    }
}