using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoService.LogicaNegocio.Modelo
{
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
            //TODO: validaciones ci
            //por el momento lo pense en largo de la ci, pero a futuro puede cambiar 
            //el largo, ya que en algun momento se van a sobrepasar los 8 numeros en la ci, pensar eso

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
