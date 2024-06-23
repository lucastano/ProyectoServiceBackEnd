using System.ComponentModel.DataAnnotations;

namespace ProyectoService.ApiRest.DTOs
{
    public class ClienteDTO
    {
        
        public int Id { get; set; } 
        public string Nombre { get; set; }
      
        public string Apellido { get; set; }

        public string Email { get; set; }

        public string Direccion { get; set; }
        
        public string Telefono { get; set; }
        public string Rol {  get; set; }
        
        public string Ci { get; set; }
    }
}
