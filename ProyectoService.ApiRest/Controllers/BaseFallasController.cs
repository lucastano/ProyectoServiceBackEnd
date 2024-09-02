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

    public class BaseFallasController : ControllerBase
    {
        private readonly IObtenerBaseFallas obtenerBaseFallasUc;
        private readonly IObtenerBaseFallaSegunDescripcion obtenerBaseFallaSegunDescripcionUc;
        private readonly IAgregarABaseFallas agregarABaseFallasUc;
        private readonly IObtenerProductoPorId obtenerProductoPorIdUc;

        public BaseFallasController(IObtenerBaseFallas obtenerBaseFallasUc, IObtenerBaseFallaSegunDescripcion obtenerBaseFallaSegunDescripcionUc, IAgregarABaseFallas agregarABaseFallasUc, IObtenerProductoPorId obtenerProductoPorIdUc)
        {
            this.obtenerBaseFallasUc = obtenerBaseFallasUc;
            this.obtenerBaseFallaSegunDescripcionUc = obtenerBaseFallaSegunDescripcionUc;
            this.agregarABaseFallasUc = agregarABaseFallasUc;
            this.obtenerProductoPorIdUc = obtenerProductoPorIdUc;
        }

        [HttpPost]
        public async Task<ActionResult> Agregar(AgregarBaseFallaDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) throw new Exception("Falta algun dato");
                Producto producto = await obtenerProductoPorIdUc.Ejecutar(dto.productoId);
                if (producto == null) throw new Exception("Debe ingresar producto");
                BaseFalla bf = new BaseFalla()
                {
                    Producto = producto,
                    Falla=dto.Falla.ToLower(),
                    Solucion=dto.Solucion.ToLower()
                    

                };
                await agregarABaseFallasUc.Ejecutar(bf);
                return Ok(bf);

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult> ObtenerTodos()
        {
            try
            {
                List<BaseFalla> bf= await obtenerBaseFallasUc.Ejecutar();
                IEnumerable<BaseFallaDTO> bfDTO = bf.Select( b=> new BaseFallaDTO()
                {
                    Id=b.Id,
                    Producto= new ProductoDTO()
                    {
                        Id=b.Id,
                        Marca=b.Producto.Marca,
                        Modelo=b.Producto.Modelo,
                        Version=b.Producto.Version

                    },
                    Falla=b.Falla,
                    Solucion = b.Solucion
                });

                return Ok(bfDTO);


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ObtenerSegunDescripcion")]
        public async Task<ActionResult> ObtenerPorFalla(string descripcion)
        {
            try
            {
                List<BaseFalla> bf = await obtenerBaseFallaSegunDescripcionUc.Ejecutar(descripcion.ToLower());
                IEnumerable<BaseFallaDTO> bfDTO = bf.Select(b => new BaseFallaDTO()
                {
                    Id = b.Id,
                    Producto = new ProductoDTO()
                    {
                        Marca = b.Producto.Marca,
                        Modelo = b.Producto.Modelo,
                        Version = b.Producto.Version

                    },
                    Falla = b.Falla,
                    Solucion = b.Solucion
                });

                return Ok(bfDTO);


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
