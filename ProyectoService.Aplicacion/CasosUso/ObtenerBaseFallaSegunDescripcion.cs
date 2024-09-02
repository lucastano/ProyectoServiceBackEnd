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
    public class ObtenerBaseFallaSegunDescripcion : IObtenerBaseFallaSegunDescripcion
    {

        private readonly IBaseFallaRepositorio repo;
        public ObtenerBaseFallaSegunDescripcion(IBaseFallaRepositorio repo)
        {
            this.repo = repo;
        }

        public async Task<List<BaseFalla>> Ejecutar(string descripcion)
        {
            return await repo.ObtenerSegunDescripcion(descripcion);
        }
    }
}
