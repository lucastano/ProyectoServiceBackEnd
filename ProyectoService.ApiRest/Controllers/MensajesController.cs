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

    public class MensajesController : ControllerBase
    {
        private readonly INuevoMensaje nuevoMensajeUc;
        private readonly IObtenerMensajes obtenerMensajesUc;
        private readonly IObtenerReparacionPorId obtenerReparacionPorIdUc;
        
             
        

        public MensajesController(INuevoMensaje nuevoMensajeUc, IObtenerMensajes obtenerMensajesUc,IObtenerReparacionPorId obtenerReparacionPorIdUc)
        {
            this.nuevoMensajeUc = nuevoMensajeUc;
            this.obtenerMensajesUc = obtenerMensajesUc;
            this.obtenerReparacionPorIdUc=obtenerReparacionPorIdUc;
            
        }


        [HttpPost]

        public async Task<ActionResult> NuevoMensaje(NuevoMensajeDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) throw new Exception("Algun campo vacio");
                
                Mensaje mensaje = new Mensaje()
                {
                    EmisorId = dto.EmisorId,
                    DestinatarioId = dto.DestinatarioId,
                    FechaHoraEnvio = DateTime.Now,
                    ReparacionId = dto.ReparacionId,
                    Texto = dto.Texto
                };

                await nuevoMensajeUc.Ejecutar(mensaje);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);  
            }

        }

        [HttpGet]

        public async Task<ActionResult>ObtenerMensajesReparacion(int id)
        {
            try
            {
                if (id == 0) throw new Exception("Reparacion no existe");
                List<Mensaje> mensajesReparacion = await obtenerMensajesUc.Ejecutar(id);
                if (mensajesReparacion.Count <= 0)
                {
                    return StatusCode(200, "Esta reparacion no tiene ningun mensaje");
                }
                Reparacion reparacion = await obtenerReparacionPorIdUc.Ejecutar(id);
                ProductoDTO producto = new ProductoDTO()
                {
                    Id = reparacion.Producto.Id,
                    Marca = reparacion.Producto.Marca,
                    Modelo=reparacion.Producto.Modelo,
                    Version=reparacion.Producto.Version
                    

                };
                ReparacionDTO repDto = new ReparacionDTO()
                {
                    Id=reparacion.Id,
                    Descripcion=reparacion.Descripcion,
                    Estado=reparacion.Estado,
                    NumeroSerie=reparacion.NumeroSerie,
                    Fecha=reparacion.Fecha,
                    Producto= producto


                };
                IEnumerable<MensajeDTO> listaMensajes = mensajesReparacion.Select(m => new MensajeDTO()
                {

                    Texto = m.Texto,
                    EmisorId = m.EmisorId,
                    EmisorNombre = m.Emisor.Nombre,
                    EmisorRol = m.Emisor.Rol,
                    DestinatarioId = m.DestinatarioId,
                    DestinatarioNombre = m.Destinatario.Nombre,
                    DestinatarioRol = m.Destinatario.Rol,
                    FechaHora = m.FechaHoraEnvio

                });
                ResponseObtenerMensajesDTO response = new ResponseObtenerMensajesDTO()
                {
                   Reparacion=repDto,
                   Mensajes=listaMensajes


                };
                return Ok(response);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


    }
}
