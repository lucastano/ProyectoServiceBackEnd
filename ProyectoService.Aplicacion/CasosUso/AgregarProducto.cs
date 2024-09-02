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
    public class AgregarProducto : IAgregarProducto
    {
        private readonly IProductoRepositorio repo;
        public AgregarProducto(IProductoRepositorio repo)
        {
            this.repo = repo;
        }

        public async Task Ejecutar(Producto producto)
        {
            await repo.Add(producto);
        }
    }
}
