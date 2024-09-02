namespace ProyectoService.ApiRest.DTOs
{
    public class BaseFallaDTO
    {
        public int Id { get; set; }
        public ProductoDTO Producto { get; set; }
        public string Falla { get; set; }
        public string Solucion { get; set; }
    }
}
