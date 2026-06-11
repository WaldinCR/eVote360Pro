using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace eVote360Pro.Core.Application.ViewModels.Candidate
{
    public class SaveCandidateViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del candidato es obligatorio.")]
        [Display(Name = "Nombre")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "El apellido del candidato es obligatorio.")]
        [Display(Name = "Apellido")]
        public string LastName { get; set; } = null!;

        [DataType(DataType.Upload)]
        [Display(Name = "Foto del Candidato")]
        public IFormFile? PhotoFile { get; set; }

        public string? PhotoUrl { get; set; }

        [Display(Name = "Estado (Activo/Inactivo)")]
        public bool IsActive { get; set; } = true;

        [Required(ErrorMessage = "El partido político es obligatorio.")]
        [Display(Name = "Partido Político")]
        public int PoliticalPartyId { get; set; }
    }
}