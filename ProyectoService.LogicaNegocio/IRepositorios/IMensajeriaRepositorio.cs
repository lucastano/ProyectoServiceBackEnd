using ProyectoService.LogicaNegocio.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoService.LogicaNegocio.IRepositorios
{
    public interface IMensajeriaRepositorio
    {
        Task<List<Mensaje>> ObtenerMensajesDeReparacion(int idReparacion);

        Task NuevoMensaje(Mensaje msg);
        Task EliminarMensajesReparacion(int idReparacion);
    }
}
