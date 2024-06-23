namespace ProyectoService.ApiRest.DTOs
{
    public class ResponseAgregarAdministradorDTO
    {
        public int StatusCode {  get; set; }
        public AgregarAdministradorDTO administradorDTO { get; set; }
        public string Error { get; set; }
    }
}
