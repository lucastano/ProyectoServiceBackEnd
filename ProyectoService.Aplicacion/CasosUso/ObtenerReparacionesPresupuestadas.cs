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
    public class ObtenerReparacionesPresupuestadas : IObtenerReparacionesPresupuestadas
    {
        private readonly IReparacionRepositorio repo;
        public ObtenerReparacionesPresupuestadas(IReparacionRepositorio repo)
        {
            this.repo = repo;
        }

        public async Task<List<Reparacion>> Ejecutar()
        {
            return await repo.ObtenerReparacionesPresupuestadas();
        }
    }
}
