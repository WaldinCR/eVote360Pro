using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eVote360Pro.Core.Application.ViewModels.PoliticalParty
{
    public class PoliticalPartyViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Nombre del Partido")]
        public string Name { get; set; } = null!;

        [Display(Name = "Descripción")]
        public string? Description { get; set; }

        [Display(Name = "Siglas")]
        public string Acronym { get; set; } = null!;

        [Display(Name = "Logo")]
        public string LogoUrl { get; set; } = null!;

        [Display(Name = "Estado")]
        public bool IsActive { get; set; }
    }
}