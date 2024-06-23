namespace ProyectoService.ApiRest.DTOs
{
    public class ResponseObtenerTecnicosDTO
    {
        public int StatusCode { get; set; }
        public List<TecnicoDTO> Tecnicos { get; set; }
    }
}
