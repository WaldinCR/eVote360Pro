using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace eVote360Pro.Core.Application.ViewModels.PoliticalLeaderAssignment
{
    public class SavePoliticalLeaderAssignmentViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un dirigente (usuario).")]
        [Display(Name = "Dirigente Político")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un partido político.")]
        [Display(Name = "Partido Político")]
        public int PoliticalPartyId { get; set; }
    }
}
