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
    public class ObtenerReparacionesPorCliente : IObtenerReparacionesPorCliente
    {
        private readonly IReparacionRepositorio repo;

        public ObtenerReparacionesPorCliente(IReparacionRepositorio repo)
        {
            this.repo = repo;
        }

        public async Task<List<Reparacion>> Ejecutar(string ci)
        {

            if (ci == null) throw new Exception("falta ci del cliente");
            return await repo.ObtenerReparacionesPorCliente(ci);
        }
    }
}
