using System.ComponentModel.DataAnnotations;

namespace ProyectoService.ApiRest.DTOs
{
    public class AgregarBaseFallaDTO
    {
        [Required]
       public int productoId {  get; set; }
        [Required]

        public string Falla { get; set; }
        [Required]

        public string Solucion { get; set; } 
    }
}
