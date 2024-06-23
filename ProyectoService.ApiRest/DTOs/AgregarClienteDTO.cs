using System.ComponentModel.DataAnnotations;

namespace ProyectoService.ApiRest.DTOs
{
    public class AgregarClienteDTO
    {
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellido { get; set; }
        [Required]

        public string Email { get; set; }
        
        public string Direccion { get; set; }
        [Required]
        public string Telefono { get; set; }
        [Required]
        public string Ci { get; set; }
    }
}
