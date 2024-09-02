using System.ComponentModel.DataAnnotations;

namespace ProyectoService.ApiRest.DTOs
{
    public class AgregarProductoDTO
    {
        [Required(ErrorMessage ="Campo marca requerido")]
        public string Marca { get; set; }
        [Required(ErrorMessage ="Campo modelo requerido")]
        

        public string Modelo { get; set; }
        [Required(ErrorMessage = "Campo version requerido")]


        public string Version { get; set; }
    }
}
