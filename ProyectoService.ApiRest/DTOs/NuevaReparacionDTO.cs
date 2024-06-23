
using ProyectoService.LogicaNegocio.Modelo;
using System.ComponentModel.DataAnnotations;

namespace ProyectoService.ApiRest.DTOs
{
    public class NuevaReparacionDTO
    {

        [Required]
        public string CiCliente { get; set; }
        [Required]
        public int IdTecnico { get; set; }//en el front, este dato lo deberiamos tener en la session
        

        public string Producto { get; set; }//esto va a pasar a ser un objeto
        [Required]
        public string NumeroSerie { get; set; }
        [Required]
        public string Descripcion { get; set; }
        public DateTime FechaPromesaPresupuesto { get; set; }
        
      
       
    }
}
