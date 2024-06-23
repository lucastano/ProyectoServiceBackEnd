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
    public class TerminarReparacion : ITerminarReparacion
    {
        private readonly IReparacionRepositorio repo;
        public TerminarReparacion(IReparacionRepositorio repo)
        {
            this.repo = repo;
        }

        public async Task<Reparacion> Ejecutar(int id, bool reparada)
        {
            //bool reparada indica si se reparo o no 
            if (id == 0) throw new Exception("debe ingresar numero de orden");
            return await repo.Terminar(id, reparada);
        }
    }
}
