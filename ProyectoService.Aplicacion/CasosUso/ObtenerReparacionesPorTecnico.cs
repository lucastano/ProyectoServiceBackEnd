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
    public class ObtenerReparacionesPorTecnico : IObtenerReparacionesPorTecnico
    {
        private readonly IReparacionRepositorio repo;
        public ObtenerReparacionesPorTecnico(IReparacionRepositorio repo)
        {
            this.repo = repo;
        }

        public async Task<List<Reparacion>> Ejecutar(string email)
        {
            if (email == null) throw new Exception("Debe ingresar un email");
            return await repo.ObtenerReparacionesPorTecnico(email);
        }
    }
}
