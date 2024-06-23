namespace ProyectoService.ApiRest.DTOs
{
    public class ResponseAgregarTecnicoDTO
    {
        public int statusCode { get; set; }
        public AgregarTecnicoDTO Tecnico { get; set; }
    }
}
