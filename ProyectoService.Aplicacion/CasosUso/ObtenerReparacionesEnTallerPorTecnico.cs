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
    public class ObtenerReparacionesEnTallerPorTecnico: IObtenerReparacionesEnTallerPorTecnico
    {
        private readonly IReparacionRepositorio repo;
        public ObtenerReparacionesEnTallerPorTecnico(IReparacionRepositorio repo)
        {
            this.repo = repo;
        }

        public async Task<List<Reparacion>> Ejecutar(string Email)
        {
            if (Email == null) throw new Exception("Debe ingresar email del tecnico");
            return await repo.ObtenerReparacionesEnTallerPorTecnico(Email);
        }
    }
}
