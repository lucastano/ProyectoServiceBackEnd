using System.ComponentModel.DataAnnotations;

namespace ProyectoService.ApiRest.DTOs
{
    public class ModificarPresupuestoReparacionDTO
    {
        [Required]
        public int Id { get; set; }
        public double Costo { get; set; }
        public string Descripcion { get; set; }

        
    }
}
