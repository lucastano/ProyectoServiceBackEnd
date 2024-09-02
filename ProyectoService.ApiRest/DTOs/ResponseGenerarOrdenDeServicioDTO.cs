
namespace ProyectoService.ApiRest.DTOs
{
    public class ResponseGenerarOrdenDeServicioDTO
    {
        public int statusCode { get; set; }

        public byte[] OrdenDeServicio { get; set; }
    }
}
