using System.ComponentModel.DataAnnotations;

namespace eVote360Pro.Core.Application.ViewModels.Login
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El nombre de usuario es requerido.")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es requerida.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}