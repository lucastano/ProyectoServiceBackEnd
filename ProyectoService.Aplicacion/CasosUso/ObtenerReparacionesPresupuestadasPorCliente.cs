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
    public class ObtenerReparacionesPresupuestadasPorCliente : IObtenerReparacionesPresupuestadasPorCliente
    {
        private readonly IReparacionRepositorio repo;
        public ObtenerReparacionesPresupuestadasPorCliente(IReparacionRepositorio repo)
        {
            this.repo = repo;
        }

        public async Task<List<Reparacion>> Ejecutar(string ci)
        {
            return await repo.ObtenerReparacionesPresupuestadasPorCliente(ci);
        }
    }
}
