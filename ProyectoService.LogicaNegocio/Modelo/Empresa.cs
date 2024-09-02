using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoService.LogicaNegocio.Modelo
{
    
    public class Empresa
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Direccion {  get; set; }
        public string Email { get; set; }
        
        public string EmailPassword { get; set; }
        public string PoliticasEmpresa {  get; set; }
        public byte []? Foto { get; set; }


        
        

    }
}
