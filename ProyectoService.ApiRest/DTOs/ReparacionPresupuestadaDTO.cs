namespace ProyectoService.ApiRest.DTOs
{
    public class ReparacionPresupuestadaDTO
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }

        public string ClienteNombre { get; set; }
        public string ClienteApellido { get; set; }
        public string ClienteCedula { get; set; }
        public string ClienteEmail { get; set; }
        public string ClienteTelefono { get; set; }
        public string ClienteDireccion { get; set; }
        public string Producto { get; set; }
        public string NumeroSerie { get; set; }
        public string Descripcion { get; set; }
        public string DescripcionPresupuesto { get; set; }
        public double ManoDeObra { get; set; }
        public double CostoFinal { get; set; }
    }
}
