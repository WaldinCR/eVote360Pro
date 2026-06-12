using eVote360Pro.Core.Application.Dtos.Citizen;
using eVote360Pro.Core.Application.Dtos.Vote;
using eVote360Pro.Core.Domain.Common.Enums;
using Microsoft.AspNetCore.Http;

namespace eVote360Pro.Core.Application.Interfaces
{
    public interface IElectorService
    {
        Task<bool> GenerateAndSendOtpAsync(int citizenId, int electionId, string citizenName, string email);
        Task<OtpValidationResult> ValidateOtpAsync(int citizenId, int electionId, string code);
        Task<bool> ConfirmVoteAsync(int citizenId, int electionId, List<SaveVoteDto> votes, string citizenName, string electionName, string electionDate, string email);
    }
}