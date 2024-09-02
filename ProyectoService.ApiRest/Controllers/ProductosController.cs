using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoService.ApiRest.DTOs;
using ProyectoService.Aplicacion.ICasosUso;
using ProyectoService.LogicaNegocio.Modelo;

namespace ProyectoService.ApiRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]


    public class ProductosController : ControllerBase
    {
        private readonly IAgregarProducto agregarProductoUc;
        private readonly IObtenerProductos obtenerProductosUc;

        public ProductosController(IAgregarProducto agregarProductoUc, IObtenerProductos obtenerProductosUc)
        {
            this.agregarProductoUc = agregarProductoUc;
            this.obtenerProductosUc = obtenerProductosUc;
        }

        [HttpPost]

        public async Task<ActionResult> AgregarProducto(AgregarProductoDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) throw new Exception("algun campo no se lleno");
                Producto producto = new Producto()
                {
                    Marca = dto.Marca,
                    Modelo = dto.Modelo,
                    Version = dto.Version

                };
                await agregarProductoUc.Ejecutar(producto);
                return Ok(producto);


            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);

            }

        }

        [HttpGet]
        public async Task<ActionResult> ObtenerTodosLosProductos()
        {
            List<Producto> listadoProductos = await obtenerProductosUc.Ejecutar();
            IEnumerable<ProductoDTO> listaRetorno = listadoProductos.Select(p => new ProductoDTO()
            {
                Id = p.Id,
                Marca = p.Marca,
                Modelo = p.Modelo,
                Version = p.Version

            });

            return Ok(listaRetorno);
        }
    }
}
