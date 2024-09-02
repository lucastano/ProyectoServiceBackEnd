namespace ProyectoService.ApiRest.DTOs
{
    public class ReparacionHCDTO
    {
        public DateTime FechaEntregaReparacion {  get; set; }
        public string DescripcionProblema { get; set; }
        public string DescripcionSolucion { get; set; }
        public double CostoReparacion { get; set; }
    }
}
