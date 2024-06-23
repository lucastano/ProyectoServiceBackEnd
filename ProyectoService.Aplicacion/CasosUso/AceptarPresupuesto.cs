using ProyectoService.Aplicacion.ICasosUso;
using ProyectoService.LogicaNegocio.IRepositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoService.Aplicacion.CasosUso
{
    public class AceptarPresupuesto : IAceptarPresupuesto
    {
        private readonly IReparacionRepositorio repo;
        public AceptarPresupuesto(IReparacionRepositorio repo)
        {
            this.repo = repo;
        }

        public async Task Ejecutar(int id)
        {
            await repo.AceptarPresupuesto(id);
        }
    }
}
