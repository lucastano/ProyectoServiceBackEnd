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
    public class ObtenerMensajes : IObtenerMensajes
    {

        private readonly IMensajeriaRepositorio repo;

        public ObtenerMensajes(IMensajeriaRepositorio repo)
        {
            this.repo = repo;
        }

        public async Task<List<Mensaje>> Ejecutar(int id)
        {
            return await repo.ObtenerMensajesDeReparacion(id);

        }
    }
}
