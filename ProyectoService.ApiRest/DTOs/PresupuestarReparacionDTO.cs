namespace ProyectoService.ApiRest.DTOs
{
    public class PresupuestarReparacionDTO
    {
        public int Id {  get; set; }
        public double ManoObra {  get; set; }
        public string Descripcion { get; set; }
       
        public DateTime FechaPromesaEntrega { get; set; }
    }
}
