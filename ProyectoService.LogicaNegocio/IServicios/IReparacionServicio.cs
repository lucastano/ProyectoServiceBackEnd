using ProyectoService.LogicaNegocio.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoService.LogicaNegocio.IServicios
{
    public interface IReparacionServicio
    {
        Task<Reparacion> ExisteReparacion(int id);
    }
}
