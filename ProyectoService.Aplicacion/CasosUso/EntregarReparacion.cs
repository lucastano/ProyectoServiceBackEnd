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
    public class EntregarReparacion : IEntregarReparacion
    {
        private readonly IReparacionRepositorio repo;
        public EntregarReparacion(IReparacionRepositorio repo)
        {
            this.repo = repo;
        }

        public async Task<Reparacion> Ejecutar(int id)
        {
            if (id == 0) throw new Exception("Numero de orden incorrecto");
            return await repo.Entregar(id);
        }
    }
}
