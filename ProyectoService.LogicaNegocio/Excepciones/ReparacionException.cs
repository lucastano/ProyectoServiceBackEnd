using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoService.LogicaNegocio.Excepciones
{
    public class ReparacionException:Exception
    {
        public ReparacionException() { }
        public ReparacionException(string message) : base(message) { }
        public ReparacionException(string message, Exception exception) : base(message) { }
    }
}
