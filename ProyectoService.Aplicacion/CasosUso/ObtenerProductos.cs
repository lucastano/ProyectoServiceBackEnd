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
    public class ObtenerProductos : IObtenerProductos
    {
        private readonly IProductoRepositorio repo;
        public ObtenerProductos(IProductoRepositorio repo)
        {
            this.repo = repo;
        }

        public async Task<List<Producto>> Ejecutar()
        {
            return await repo.getAll();

        }
    }
}
