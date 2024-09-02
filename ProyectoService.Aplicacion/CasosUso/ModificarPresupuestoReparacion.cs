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
    public class ModificarPresupuestoReparacion : IModificarPresupuestoReparacion
    {
        private readonly IReparacionRepositorio repo;
        public ModificarPresupuestoReparacion(IReparacionRepositorio repo)
        {
            this.repo = repo;
        }

        public async Task<Reparacion> Ejecutar(int id, double costo, string descripcion)
        {
            return await repo.ModificarPresupuestoReparacion(id, costo,descripcion);
        }
    }
}
