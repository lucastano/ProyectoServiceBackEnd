using ProyectoService.LogicaNegocio.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoService.LogicaNegocio.IRepositorios
{
    public interface IEnviarEmail
    {
        Task<byte[]> EnviarEmailNuevaReparacion(Reparacion entity);
        Task EnviarEmailNuevoPresupuesto(Reparacion entity);
        Task EnviarEmailAvisoTerminada(Reparacion entity);
        Task<byte[]> EnviarEmailAvisoEntrega(Reparacion entity);
        Task AvisoCambioPassword(Usuario usu,string password);

    }
}
