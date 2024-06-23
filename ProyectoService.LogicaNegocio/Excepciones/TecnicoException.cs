
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoService.LogicaNegocio.Excepciones
{
    public class TecnicoException:Exception
    {

        public TecnicoException() { }
        public TecnicoException(string message) : base(message) { }
        public TecnicoException(string message, Exception exception) : base(message) { }
    }
}
