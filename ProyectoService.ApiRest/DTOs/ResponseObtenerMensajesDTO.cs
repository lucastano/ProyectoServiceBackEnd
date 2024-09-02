namespace ProyectoService.ApiRest.DTOs
{
    public class ResponseObtenerMensajesDTO
    {
        public ReparacionDTO Reparacion {  get; set; }
        public IEnumerable<MensajeDTO>Mensajes { get; set; }
    }
}
