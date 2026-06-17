using eVote360Pro.Core.Application.ViewModels.PoliticalParty;
using eVote360Pro.Core.Application.ViewModels.User;
using System.ComponentModel.DataAnnotations;


namespace eVote360Pro.Core.Application.ViewModels.PoliticalLeader
{
    public class SavePoliticalLeaderViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un usuario como dirigente.")]
        [Display(Name = "Usuario Dirigente")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un partido político.")]
        [Display(Name = "Partido Político")]
        public int PoliticalPartyId { get; set; }

        // Listas
        public List<PoliticalPartyViewModel>? AvailableParties { get; set; }
        public List<UserViewModel>? AvailableUsers { get; set; }
    }
}