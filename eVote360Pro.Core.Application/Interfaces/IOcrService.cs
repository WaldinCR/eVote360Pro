
using Microsoft.AspNetCore.Http;

namespace eVote360Pro.Core.Application.Interfaces
{
    public interface IOcrService
    {
        bool IsValidImageFormat(IFormFile image);
        Task<string?> ExtractDocumentNumberAsync(IFormFile image);
    }
}
