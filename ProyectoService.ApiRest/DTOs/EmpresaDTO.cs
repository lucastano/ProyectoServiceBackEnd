namespace ProyectoService.ApiRest.DTOs
{
    public class EmpresaDTO
    {
        
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string Email { get; set; }

        public string EmailPassword { get; set; }
        public byte[] Foto { get; set; }
    }
}
