using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eVote360Pro.Core.Application.ViewModels.PoliticalParty
{
    public class SavePoliticalPartyViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del partido es obligatorio.")]
        [Display(Name = "Nombre del Partido")]
        public string Name { get; set; } = null!;

        [Display(Name = "Descripción")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Las siglas son obligatorias.")]
        [Display(Name = "Siglas")]
        public string Acronym { get; set; } = null!;

        [DataType(DataType.Upload)]
        [Display(Name = "Logo del Partido")]
        public IFormFile? LogoFile { get; set; } 

        public string? LogoUrl { get; set; } 

        [Display(Name = "Estado (Activo/Inactivo)")]
        public bool IsActive { get; set; } = true;
    }
}