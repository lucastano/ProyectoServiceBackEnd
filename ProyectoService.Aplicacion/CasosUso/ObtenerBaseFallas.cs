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
    public class ObtenerBaseFallas : IObtenerBaseFallas
    {
        private readonly IBaseFallaRepositorio repo;
        public ObtenerBaseFallas(IBaseFallaRepositorio repo)
        {
            this.repo = repo;
        }

        public async Task<List<BaseFalla>> Ejecutar()
        {
            return await repo.getAll();
        }
    }
}
