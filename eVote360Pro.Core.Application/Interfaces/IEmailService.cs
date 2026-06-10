using eVote360Pro.Core.Application.Dtos.Email;

namespace eVote360Pro.Core.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequestDto emailRequestDto);
    }
}