using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eVote360Pro.Core.Application.ViewModels.Leader
{
    public class LeaderHomeViewModel
    {
        [Display(Name = "Nombre del Partido")]
        public string PartyName { get; set; } = null!;

        [Display(Name = "Siglas")]
        public string PartyAcronym { get; set; } = null!;

        [Display(Name = "Logo")]
        public string PartyLogoUrl { get; set; } = null!;

        // Indicadores
        public int ActiveCandidatesCount { get; set; }
        public int InactiveCandidatesCount { get; set; }
        public int ApprovedAlliancesCount { get; set; }
        public int PendingAllianceRequestsCount { get; set; }
    }
}