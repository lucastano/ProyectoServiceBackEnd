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
    public class ObtenerReparacionesEntregadas : IObtenerReparacionesEntregadas
    {
        private readonly IReparacionRepositorio repo;
        public ObtenerReparacionesEntregadas(IReparacionRepositorio repo)
        {
            this.repo = repo;
        }

        public async Task<List<Reparacion>> Ejecutar()
        {
            return await repo.ObtenerReparacionesEntregadas();
        }
    }
}
