using System.Collections.Generic;

namespace eVote360Pro.Core.Application.ViewModels.Election
{
    public class ElectionResultViewModel
    {
        public int ElectionId { get; set; }
        public string ElectionName { get; set; } = null!;
        public int Year { get; set; }
        public List<PositionResultViewModel> Positions { get; set; } = new();
    }

    public class PositionResultViewModel
    {
        public string PositionName { get; set; } = null!;
        public int TotalVotes { get; set; }
        public List<CandidateResultViewModel> Candidates { get; set; } = new();
        
        // Propiedades para manejar los empates y el ganador
        public bool IsTie { get; set; }
        public string WinnerName { get; set; } = null!;
    }

    public class CandidateResultViewModel
    {
        public string CandidateName { get; set; } = null!;
        public string PartyName { get; set; } = null!;
        public int Votes { get; set; }
        public double Percentage { get; set; }
    }
}