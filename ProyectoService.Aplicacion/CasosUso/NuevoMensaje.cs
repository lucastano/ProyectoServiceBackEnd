using ProyectoService.Aplicacion.ICasosUso;
using ProyectoService.LogicaNegocio.IRepositorios;
using ProyectoService.LogicaNegocio.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoService.Aplicacion.CasosUso
{
    public class NuevoMensaje : INuevoMensaje
    {
        private readonly IMensajeriaRepositorio repo;
        public NuevoMensaje(IMensajeriaRepositorio repo)
        {
            this.repo = repo;
        }

        public async Task Ejecutar(Mensaje msg)
        {
            await repo.NuevoMensaje(msg);
        }
    }
}
