namespace ProyectoService.ApiRest.DTOs
{
    public class ResponseLoginDTO
    {
        public int StatusCode { get; set; }
        public string Token { get; set; }
        public UsuarioLogeadoDTO Usuario { get; set; }
        public string Error {  get; set; }
    }
}
