using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoService.LogicaNegocio.Excepciones
{
    public class AdministradorException:Exception
    {
        public AdministradorException() { }
        public AdministradorException(string message) : base(message) { }
        public AdministradorException(string message, Exception exception) : base(message) { }
    }
}
