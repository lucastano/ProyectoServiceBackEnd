namespace ProyectoService.ApiRest.DTOs
{
    public class ReparacionEnTallerDTO
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        
        public string ClienteNombre { get; set; }
        public string ClienteApellido { get; set; }
        public string ClienteCedula { get; set; }
        public string ClienteEmail { get; set; }
        public string ClienteTelefono { get; set; }
        public string ClienteDireccion { get; set; }
        
        public int TecnicoId {  get; set; }
        public ProductoDTO Producto { get; set; }
        public string NumeroSerie { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaPromesaPresupuesto { get; set; }
        public string Estado { get; set; }
        public string DescripcionPresupuesto {  get; set; }
        public double Costo { get; set; }

        public DateTime FechaPromesaEntrega { get; set; }
    }
}
