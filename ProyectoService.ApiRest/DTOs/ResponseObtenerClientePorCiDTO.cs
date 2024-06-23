namespace ProyectoService.ApiRest.DTOs
{
    public class ResponseObtenerClientePorCiDTO
    {
        public int StatusCode { get; set; }
        public ClienteDTO cliente { get; set; }
        public string Error { get; set; }
    }
}
