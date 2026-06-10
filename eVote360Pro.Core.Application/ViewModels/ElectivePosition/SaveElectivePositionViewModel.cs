using System.ComponentModel.DataAnnotations;

namespace eVote360Pro.Core.Application.ViewModels.ElectivePosition
{
    public class SaveElectivePositionViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del puesto es obligatorio.")]
        [Display(Name = "Nombre del Puesto")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [Display(Name = "Descripción")]
        public string? Description { get; set; }

        [Display(Name = "Estado")]
        public bool IsActive { get; set; } = true;

        // Propiedad auxiliar para saber si bloqueamos el campo 'Name' en la vista
        public bool HasParticipated { get; set; } 
    }
}