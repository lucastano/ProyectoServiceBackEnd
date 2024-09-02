using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoService.LogicaNegocio.Validaciones
{
    public class ValidacionesTexto
    {

        public static string FormatearTexto(string txt)
        {
            if (txt != null)
            {
                txt=txt.Substring(0,1).ToUpper()+txt.Substring(1).ToLower();
                return txt;

            }
            else
            {
                return txt;
            }

        }
    }
}
