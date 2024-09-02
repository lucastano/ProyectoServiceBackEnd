using ProyectoService.Aplicacion.ICasosUso;
using ProyectoService.LogicaNegocio.IRepositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoService.Aplicacion.CasosUso
{
    public class EliminarMensajesReparacion : IEliminarMensajesReparacion
    {
        private readonly IMensajeriaRepositorio repo;

        public EliminarMensajesReparacion(IMensajeriaRepositorio repo)
        {
            this.repo = repo;
        }

        public async Task Ejecutar(int id)
        {
            await repo.EliminarMensajesReparacion(id);
        }
    }
}
