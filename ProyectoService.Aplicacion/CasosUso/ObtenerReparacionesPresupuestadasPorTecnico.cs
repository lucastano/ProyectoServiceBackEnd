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
    public class ObtenerReparacionesPresupuestadasPorTecnico : IObtenerReparacionesPresupuestadasPorTecnico
    {
        private readonly IReparacionRepositorio repo;
        public ObtenerReparacionesPresupuestadasPorTecnico(IReparacionRepositorio repo)
        {
            this.repo = repo;
        }

        public async Task<List<Reparacion>> Ejecutar(string email)
        {
            return await repo.ObtenerReparacionesPresupuestadasPorTecnico(email);
        }
    }
}
