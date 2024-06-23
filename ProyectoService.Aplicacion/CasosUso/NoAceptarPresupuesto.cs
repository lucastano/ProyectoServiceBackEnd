using ProyectoService.Aplicacion.ICasosUso;
using ProyectoService.LogicaNegocio.IRepositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoService.Aplicacion.CasosUso
{
    public class NoAceptarPresupuesto : INoAceptarPresupuesto
    {
        private readonly IReparacionRepositorio repo;
        public NoAceptarPresupuesto(IReparacionRepositorio repo)
        {
            this.repo = repo;
        }

        public async Task Ejecutar(int id, double costo,string razon)
        {
            if (id == 0) throw new Exception("Numero de orden incorrecto");
            if (razon == "") throw new Exception("Debe ingresar una razon");
            await repo.NoAceptarPresupuesto(id,costo,razon);
        }
    }
}
