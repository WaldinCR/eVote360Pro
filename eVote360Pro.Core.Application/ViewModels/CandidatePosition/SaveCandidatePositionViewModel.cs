using System.ComponentModel.DataAnnotations;

namespace eVote360Pro.Core.Application.ViewModels.CandidatePosition
{
    public class SaveCandidatePositionViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un candidato.")]
        [Display(Name = "Candidato")]
        public int CandidateId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un puesto electivo.")]
        [Display(Name = "Puesto Electivo")]
        public int ElectivePositionId { get; set; }

        // Campo oculto para saber a qué partido (del dirigente en sesión) se le está asignando
        public int PoliticalPartyId { get; set; }
    }
}