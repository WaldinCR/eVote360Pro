using eVote360Pro.Core.Application.ViewModels.PoliticalParty;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        // Listas para los dropdowns pendiente 
        public List<PoliticalPartyViewModel>? AvailableParties { get; set; }
        //public List<UserViewModel>? AvailableUsers { get; set; }
    }
}