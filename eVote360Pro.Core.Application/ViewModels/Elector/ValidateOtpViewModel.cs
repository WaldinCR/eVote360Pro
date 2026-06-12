using System.ComponentModel.DataAnnotations;

namespace eVote360Pro.Core.Application.ViewModels.Elector
{
    public class ValidateOtpViewModel
    {
        [Required(ErrorMessage = "Debe ingresar el código de verificación.")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "El código debe tener exactamente 6 dígitos.")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "El código debe contener solo dígitos.")]
        public required string Code { get; set; }
    }
}