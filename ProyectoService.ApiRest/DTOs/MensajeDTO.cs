namespace ProyectoService.ApiRest.DTOs
{
    public class MensajeDTO
    {

        
        public int EmisorId { get; set; }

        public string EmisorNombre { get; set; }
        public string EmisorRol {  get; set; }
        public int DestinatarioId { get; set; }

        public string DestinatarioNombre { get; set; }
        public string DestinatarioRol {  get; set; }
        public string Texto {  get; set; }
        public DateTime FechaHora { get; set; }
    }
}
