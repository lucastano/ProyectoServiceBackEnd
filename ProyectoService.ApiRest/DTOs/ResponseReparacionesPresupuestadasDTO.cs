namespace ProyectoService.ApiRest.DTOs
{
    public class ResponseReparacionesPresupuestadasDTO
    {
        public int StatusCode { get; set; }
        public List<ReparacionPresupuestadaDTO>ReparacionesPresupuestadas { get; set; }
    }
}
