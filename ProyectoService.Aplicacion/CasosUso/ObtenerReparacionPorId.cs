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
    public class ObtenerReparacionPorId : IObtenerReparacionPorId
    {
        private readonly IReparacionRepositorio repo;
        public ObtenerReparacionPorId(IReparacionRepositorio repo)
        {
            this.repo = repo;
        }

        public async Task<Reparacion> Ejecutar(int id)
        {
            return await repo.ObtenerReparacionPorId(id);
        }
    }
}
