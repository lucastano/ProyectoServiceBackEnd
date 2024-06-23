
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations;

namespace ProyectoService.ApiRest.DTOs
{
    public class AgregarAdministradorDTO
    {
        [Required]
        public string Nombre {  get; set; }
        [Required]

        public string Apellido { get; set; }
        [Required]

        public string Email { get; set; }
        [Required]

        public string Password { get; set; }
    }
}
