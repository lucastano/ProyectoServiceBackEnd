using System.ComponentModel.DataAnnotations;

namespace ProyectoService.ApiRest.DTOs
{
    public class ModificarDatosReparacionDTO
    {
        [Required]
        public int Id {  get; set; }
        public DateTime FechaPromesaPresupuesto {  get; set; }
        public string NumeroSerie {  get; set; }
        public string Descripcion {  get; set; }
    }
}
