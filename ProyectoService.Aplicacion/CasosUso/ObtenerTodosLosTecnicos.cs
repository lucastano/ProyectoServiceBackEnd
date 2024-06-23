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
    public class ObtenerTodosLosTecnicos : IObtenerTodosLosTecnicos
    {
        private readonly ITecnicoRepositorio repo;
        public ObtenerTodosLosTecnicos(ITecnicoRepositorio repo)
        {
            this.repo = repo;
        }

        public async  Task<IEnumerable<Tecnico>> Ejecutar()
        {
            return await repo.getAll();
        }
    }
}
