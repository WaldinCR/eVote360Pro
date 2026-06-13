using System;
using System.ComponentModel.DataAnnotations;

namespace eVote360Pro.Core.Application.ViewModels.Election
{
    public class SaveElectionViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la elección es obligatorio.")]
        [Display(Name = "Nombre de la Elección")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "El año de la elección es obligatorio.")]
        [Display(Name = "Año Electoral")]
        public int Year { get; set; } = DateTime.Now.Year; 
    }
}