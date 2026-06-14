using System.ComponentModel.DataAnnotations;

namespace eVote360Pro.Core.Application.ViewModels.Alliance
{
    public class CreateAllianceRequestViewModel
    {
        [Required(ErrorMessage = "Debe seleccionar su partido.")]
        [Display(Name = "Su Partido (Solicitante)")]
        public int ApplicantPartyId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar el partido con el que desea aliarse.")]
        [Display(Name = "Partido a Aliarse (Receptor)")]
        public int ReceiverPartyId { get; set; }

        //public List<PoliticalPartyViewModel>? AvailableParties { get; set; }
    }
}