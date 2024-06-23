using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoService.LogicaNegocio.Modelo
{
    public class Administrador:Usuario
    {

        public Administrador() 
        {
            this.Rol = "Administrador";
        }
    }
}
