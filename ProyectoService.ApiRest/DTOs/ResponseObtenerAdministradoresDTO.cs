namespace ProyectoService.ApiRest.DTOs
{
    public class ResponseObtenerAdministradoresDTO
    {
        public int StatusCode { get; set; }
        public List<AdministradorDTO>administradores { get; set; }
        public string Error {  get; set; }
    }
}
