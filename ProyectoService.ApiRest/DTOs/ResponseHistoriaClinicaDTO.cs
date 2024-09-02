namespace ProyectoService.ApiRest.DTOs
{
    public class ResponseHistoriaClinicaDTO
    {
        
        public string NumeroSerie { get; set; }
        public int CantidadReparacionesRealizadas {  get; set; }
        public double GastoTotalEnReparaciones { get; set; }
        public IEnumerable<ReparacionHCDTO> reparacionesRealizadas { get; set; }
    }
}
