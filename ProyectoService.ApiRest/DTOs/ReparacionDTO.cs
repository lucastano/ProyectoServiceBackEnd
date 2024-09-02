using ProyectoService.LogicaNegocio.Modelo;

namespace ProyectoService.ApiRest.DTOs
{
    public class ReparacionDTO
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; } 
        public ProductoDTO Producto { get; set; }
        public string NumeroSerie { get; set; }
        public string Descripcion { get; set; }
       
        public string Estado { get; set; }
        
    }
}
