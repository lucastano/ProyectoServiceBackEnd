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
    public class AgregarReparacion : IAgregarReparacion
    {
        private readonly IReparacionRepositorio repo;
        public AgregarReparacion(IReparacionRepositorio repo)
        {
            this.repo = repo;
        }

        public async Task<Reparacion> Ejecutar(Reparacion entity)
        {
            //await repo.Add(entity);
            Reparacion rep = await repo.AddAlternativo(entity);
            return rep;

        }

        
    }
}
