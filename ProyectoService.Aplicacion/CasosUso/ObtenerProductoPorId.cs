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
    public class ObtenerProductoPorId : IObtenerProductoPorId
    {
        private readonly IProductoRepositorio repo;
        public ObtenerProductoPorId(IProductoRepositorio repo)
        {
            this.repo = repo;
        }

        public async Task<Producto> Ejecutar(int id)
        {
            return await repo.ObtenerProductoPorId(id);
        }
    }
}
