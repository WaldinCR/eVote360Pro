using eVote360Pro.Core.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Tesseract;

namespace eVote360Pro.Infrastructure.Shared.Services
{
    public class OcrService : IOcrService
    {
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png" };

        public bool IsValidImageFormat(IFormFile image)
        {
            var extension = Path.GetExtension(image.FileName).ToLowerInvariant();
            return AllowedExtensions.Contains(extension);
        }

        public async Task<string?> ExtractDocumentNumberAsync(IFormFile image)
        {
            using var ms = new MemoryStream();
            await image.CopyToAsync(ms);
            var imageBytes = ms.ToArray();

            var tessDataPath = Path.Combine(Directory.GetCurrentDirectory(), "tessdata");

            using var engine = new TesseractEngine(tessDataPath, "spa", EngineMode.Default);
            using var img = Pix.LoadFromMemory(imageBytes);
            using var page = engine.Process(img);

            return page.GetText()?.Trim();
        }
    }
}
