using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoService.LogicaNegocio.Excepciones
{
    public class ClienteException:Exception
    {
        public ClienteException() { }
        public ClienteException(string message) : base(message) { }
        public ClienteException(string message,Exception exception) : base(message) { }

    }
}
