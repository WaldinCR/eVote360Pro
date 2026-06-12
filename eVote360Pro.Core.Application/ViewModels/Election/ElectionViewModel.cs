using eVote360Pro.Core.Domain.Common.Enums;

namespace eVote360Pro.Core.Application.ViewModels.Election
{
    public class ElectionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Year { get; set; } 
        public ElectionStatus Status { get; set; }
    }
}