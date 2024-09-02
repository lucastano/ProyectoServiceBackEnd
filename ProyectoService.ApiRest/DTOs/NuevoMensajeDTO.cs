using ProyectoService.LogicaNegocio.Modelo;
using System.ComponentModel.DataAnnotations;

namespace ProyectoService.ApiRest.DTOs
{
    public class NuevoMensajeDTO
    {
        [Required(ErrorMessage = "Reparacion requerida")]
        public int ReparacionId { get; set; }
        [Required(ErrorMessage = "Emisor requerido")]


        public int EmisorId { get; set; }
        [Required(ErrorMessage = "Destinatario requerido")]

        public int DestinatarioId { get; set; }
        [Required(ErrorMessage = "Mensaje requerido")]

        public string Texto { get; set; }
    }
}
