namespace ProyectoService.ApiRest.DTOs
{
    public class ResponseAgregarClienteDTO
    {
        public int StatusCode { get; set; }
        public AgregarClienteDTO Cliente { get; set; }
        public string Error { get; set; }
    }
}
