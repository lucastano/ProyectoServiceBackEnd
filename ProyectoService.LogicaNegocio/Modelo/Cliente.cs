using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;



namespace ProyectoService.LogicaNegocio.Modelo
{
    [Table("clientes")]
    [Index(nameof(Ci),IsUnique =true,Name ="IX_Cliente_Ci")]
    public class Cliente:Usuario
    {
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Ci { get; set; }


        public Cliente() 
        {
            this.Rol = "Cliente";
        }


        public  bool validarCi()
        {
            

            bool retorno = true;
            if (this.Ci == null)
            {
                retorno = false;
            }
            if (this.Ci.Length<8)
            {
                retorno = false;
            }
            
           
            return retorno;

            
        }

        

        
    }
}
