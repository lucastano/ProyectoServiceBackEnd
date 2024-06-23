namespace ProyectoService.ApiRest.DTOs
{
    public class ResponseReparacionesEnTallerDTO
    {
        public int StatusCode { get; set; }
        public List<ReparacionEnTallerDTO> reparaciones {  get; set; }


    }
}
