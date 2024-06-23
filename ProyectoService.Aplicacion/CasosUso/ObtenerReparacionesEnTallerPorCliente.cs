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
    public class ObtenerReparacionesEnTallerPorCliente : IObtenerReparacionesEnTallerPorCliente
    {
        private readonly IReparacionRepositorio repo;
        public ObtenerReparacionesEnTallerPorCliente(IReparacionRepositorio repo)
        {
            this.repo = repo;
        }

        public async Task<List<Reparacion>> Ejecutar(string ci)
        {
            if (ci.Any()) throw new Exception("Debe ingresar ci");
            return await repo.ObtenerReparacionesEnTallerPorCliente(ci);
        }
    }
}
