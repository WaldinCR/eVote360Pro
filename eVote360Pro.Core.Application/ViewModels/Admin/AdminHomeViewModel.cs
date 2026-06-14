using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eVote360Pro.Core.Application.ViewModels.Admin
{
    public class AdminHomeViewModel
    {
        [Display(Name = "Año Seleccionado")]
        public int SelectedYear { get; set; }

        public List<int> AvailableYears { get; set; } = new List<int>();
        public List<ElectionSummaryViewModel> Elections { get; set; } = new List<ElectionSummaryViewModel>();
    }

    public class ElectionSummaryViewModel
    {
        public string ElectionName { get; set; } = null!;
        public string RealizationDate { get; set; } = null!;
        public int ParticipatingParties { get; set; }
        public int RealCandidates { get; set; }
        public int CitizensVoted { get; set; }
    }
}