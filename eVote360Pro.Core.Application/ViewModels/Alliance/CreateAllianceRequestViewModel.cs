using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using eVote360Pro.Core.Application.ViewModels.PoliticalParty;

namespace eVote360Pro.Core.Application.ViewModels.Alliance
{
    public class CreateAllianceRequestViewModel
    {
      
      [Required(ErrorMessage = "Debe seleccionar el partido político destino.")]
      public int ReceiverPartyId { get; set; }

      //public List<PoliticalPartyViewModel>? AvailableParties { get; set; }
    }
}
