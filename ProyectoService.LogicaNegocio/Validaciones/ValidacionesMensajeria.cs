using ProyectoService.LogicaNegocio.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoService.LogicaNegocio.Validaciones
{
    public class ValidacionesMensajeria
    {
        
        public static bool ValidarUsuariosReparacion(Reparacion reparacion,Usuario emisor,Usuario destinatario)
        {
            

            int usuarioEmisor = emisor.Id;
            int usuarioDestinatario=destinatario.Id;
            int tecnico = reparacion.Tecnico.Id;
            int cliente = reparacion.Cliente.Id;
            bool participantesValidos = false;
  
            if (usuarioEmisor!=cliente)
            {
                if (usuarioDestinatario == cliente)
                {
                    participantesValidos = true;
                }
                

            }
            else
            {
                participantesValidos=true;
            }
            
            return participantesValidos;
        }
    }
}
