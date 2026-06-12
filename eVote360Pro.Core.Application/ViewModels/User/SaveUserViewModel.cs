using System.ComponentModel.DataAnnotations;

namespace eVote360Pro.Core.Application.ViewModels.User
{
    public class SaveUserViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre no puede exceder los 50 caracteres.")]
        [DataType(DataType.Text)]
        public required string Name { get; set; } 

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [StringLength(50, ErrorMessage = "El apellido no puede exceder los 50 caracteres.")]
        [DataType(DataType.Text)]
        public required string LastName { get; set; } 

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "Debe introducir una dirección de correo válida.")]
        [DataType(DataType.EmailAddress)]
        public required string Email { get; set; } 

        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [StringLength(30, ErrorMessage = "El usuario debe tener entre 4 y 30 caracteres.", MinimumLength = 4)]
        [DataType(DataType.Text)]
        public required string UserName { get; set; }

        [Required(ErrorMessage = "Debe ingresar la contraseña del usuario")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "La contraseña y la confirmación no coinciden.")]
        [Required(ErrorMessage = "Debe confirmar la contraseña")]
        public required string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un rol para el usuario.")]
        public required int Role { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
