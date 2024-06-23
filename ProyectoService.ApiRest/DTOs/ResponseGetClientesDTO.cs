namespace ProyectoService.ApiRest.DTOs
{
    public class ResponseGetClientesDTO
    {
        public int StatusCode {  get; set; }
        public List<ClienteDTO> Clientes { get; set;}
    }
}
