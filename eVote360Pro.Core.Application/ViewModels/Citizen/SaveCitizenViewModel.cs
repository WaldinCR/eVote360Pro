using System.ComponentModel.DataAnnotations;

namespace eVote360Pro.Core.Application.ViewModels.Citizen
{
    public class SaveCitizenViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El número de documento es obligatorio.")]
        [Display(Name = "Número de Documento")]
        public string Document { get; set; } = null!;

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [Display(Name = "Nombre")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [Display(Name = "Apellido")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "Debe ser un correo electrónico válido.")]
        [Display(Name = "Correo Electrónico")]
        public string Email { get; set; } = null!;

        public bool IsActive { get; set; } = true;
        public bool HasVoted { get; set; } 
    }
}