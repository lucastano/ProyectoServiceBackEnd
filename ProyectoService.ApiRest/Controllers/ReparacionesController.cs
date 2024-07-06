﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoService.ApiRest.DTOs;
using ProyectoService.Aplicacion.CasosUso;
using ProyectoService.Aplicacion.ICasosUso;
using ProyectoService.LogicaNegocio.Modelo;

namespace ProyectoService.ApiRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReparacionesController : ControllerBase
    {
        private readonly IAgregarReparacion agregarReparacionUc;
        private readonly IObtenerTodasLasReparaciones obtenerTodasLasReparacionesUc;
        private readonly IObtenerReparacionesPorCliente obtenerReparacionesPorClienteUc;
        private readonly IObtenerReparacionesPorTecnico obtenerReparacionesPorTecnicoUc;
        private readonly IPresupuestarReparacion presupuestarReparacionUc;
        private readonly IAgregarClienteUC agregarClienteUc;
        private readonly IObtenerClientePorCI obtenerClientePorCiUc;
        private readonly IObtenerTecnicoPorId obtenerTecnicoPorIdUc;
        private readonly IAvisoNuevaReparacion avisoNuevaReparacionUc;
        private readonly IAvisoNuevoPresupuesto avisoNuevoPresupuestoUc;
        private readonly IAvisoEntregaReparacion avisoEntregarReparacionUc;
        private readonly IAvisoReparacionTerminada avisoReparacionTerminadaUc;
        private readonly IAceptarPresupuesto aceptarPresupuestoUc;
        private readonly INoAceptarPresupuesto noAceptarPresupuestoUc;
        private readonly ITerminarReparacion terminarReparacionUc;
        private readonly IEntregarReparacion entregarReparacionUc;
        private readonly IObtenerEmpresa obtenerEmpresaUc;



        public ReparacionesController(IAgregarReparacion agregarReparacionUc, IObtenerTodasLasReparaciones obtenerTodasLasReparacionesUc, IObtenerReparacionesPorCliente obtenerReparacionesPorClienteUc, IObtenerReparacionesPorTecnico obtenerReparacionesPorTecnicoUc, IPresupuestarReparacion presupuestarReparacionUc, IObtenerClientePorCI obtenerClientePorCiUc, IObtenerTecnicoPorId obtenerTecnicoPorIdUc, IAvisoNuevaReparacion avisoNuevaReparacionUc, IAvisoNuevoPresupuesto avisoNuevoPresupuestoUc, IAceptarPresupuesto aceptarPresupuestoUc, INoAceptarPresupuesto noAceptarPresupuestoUc, ITerminarReparacion terminarReparacionUc, IEntregarReparacion entregarReparacionUc, IAvisoEntregaReparacion avisoEntregarReparacionUc, IAvisoReparacionTerminada avisoReparacionTerminadaUc, IObtenerEmpresa obtenerEmpresaUc)
        {
            this.agregarReparacionUc = agregarReparacionUc;
            this.obtenerTodasLasReparacionesUc = obtenerTodasLasReparacionesUc;
            this.obtenerReparacionesPorClienteUc = obtenerReparacionesPorClienteUc;
            this.obtenerReparacionesPorTecnicoUc = obtenerReparacionesPorTecnicoUc;
            this.presupuestarReparacionUc = presupuestarReparacionUc;
            this.obtenerClientePorCiUc = obtenerClientePorCiUc;
            this.obtenerTecnicoPorIdUc = obtenerTecnicoPorIdUc;
            this.avisoNuevaReparacionUc = avisoNuevaReparacionUc;
            this.avisoNuevoPresupuestoUc = avisoNuevoPresupuestoUc;
            this.aceptarPresupuestoUc = aceptarPresupuestoUc;
            this.noAceptarPresupuestoUc = noAceptarPresupuestoUc;
            this.terminarReparacionUc = terminarReparacionUc;
            this.entregarReparacionUc = entregarReparacionUc;
            this.avisoEntregarReparacionUc = avisoEntregarReparacionUc;
            this.avisoReparacionTerminadaUc = avisoReparacionTerminadaUc;
            this.obtenerEmpresaUc = obtenerEmpresaUc;
        }

        [HttpPost]

        public async Task<ActionResult> NuevaReparacion(NuevaReparacionDTO dto)
        {
            if (!ModelState.IsValid) throw new Exception("Debe ingresar datos de la reparacion");
            try
            {
                //PARA OBTENER EL TECNICO UTILIZO DATOS DE LA SESSION
                Empresa empresa = await obtenerEmpresaUc.Ejecutar();
                Tecnico tecnico = await obtenerTecnicoPorIdUc.Ejecutar(dto.IdTecnico);
                if (tecnico == null) throw new Exception("Tecnico no existe");
                //el ccliente debe existir, en la realidad, no se van a agregar clientes individualmente,
                //sino que con cada servicio, se ingresan los datos del cliente y ahi se agrega
                // la idea es al crear un nuevo servicio, ingresar LA CI DEL CLIENTE, lo busca por ci, si encuentra, retorna los datos del cliente.
                // si no, pide que ingrese los datos, TODO ESO LO MANEJA EL FRONT.
                // ESTE ENDPOINT REQUIERE QUE EL FRONT, LE DE LA CI DEL CLIENTE EXISTENTE.  
                Cliente cliente = await obtenerClientePorCiUc.Ejecutar(dto.CiCliente);
                Reparacion reparacion = new Reparacion()
                {
                    Tecnico = tecnico,
                    Cliente = cliente,
                    Producto = dto.Producto,
                    NumeroSerie = dto.NumeroSerie,
                    Descripcion = dto.Descripcion,
                    FechaPromesaPresupuesto=dto.FechaPromesaPresupuesto

                };

               Reparacion rep= await agregarReparacionUc.Ejecutar(reparacion);
               await avisoNuevaReparacionUc.Ejecutar(rep);
                
                //Byte[] pdf = rep.GenerarPdfOrdenServicioEntrada();
                // rep.GenerarPdfOrdenServicioEntrada();
                return Ok();




            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

            
        }

        [HttpPost("Presupuestar")]
        public async Task<ActionResult> PresupuestarReparacion(PresupuestarReparacionDTO dto)
        {
            try
            {
                if (dto.Id == 0) throw new Exception("No existe reparacion con ese id");
                if (dto.Descripcion == null) throw new Exception("Debe ingresar una descripcio");
                Reparacion rep=await presupuestarReparacionUc.Ejecutar(dto.Id,dto.ManoObra,dto.Descripcion,dto.FechaPromesaEntrega);
                await avisoNuevoPresupuestoUc.Ejecutar(rep);//caso de uso 
                return StatusCode(200);    

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        [HttpPost("AceptarPresupuesto")]
        public async Task<ActionResult>AceptarPresupuesto(int id)
        {
            try
            {
                if (id == 0) throw new Exception("Numero de orden incorrecto");
                await aceptarPresupuestoUc.Ejecutar(id);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }
        [HttpPost("NoAceptarPresupuesto")]
        public async Task<ActionResult> NoAceptarPresupuesto(int id,double costo,string razon)
        {
            try
            {
                if (id == 0) throw new Exception("Numero de orden incorrecto");
                await noAceptarPresupuestoUc.Ejecutar(id,costo,razon);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        [HttpPost("TerminarReparacion")]
        public async Task<ActionResult<Reparacion>> TerminarReparacion(int id, bool reparada)
        {
            try
            {
                if (id == 0) throw new Exception("Numero de orden incorrecto");
                Reparacion reparacion = await terminarReparacionUc.Ejecutar(id,reparada);
                if (reparacion == null) throw new Exception("No se pudo terminar esta reparacion");
                await avisoReparacionTerminadaUc.Ejecutar(reparacion);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message) ;

            }


        }

        [HttpPost("EntregarReparacion")]
        public async Task<ActionResult<Reparacion>> EntregarReparacion(int id)
        {
            try
            {
                if (id == 0) throw new Exception("Numero de orden incorrecto");
                Reparacion reparacion = await entregarReparacionUc.Ejecutar(id);
                if (reparacion == null) throw new Exception("No se pudo entregar esta reparacion");
                await avisoEntregarReparacionUc.Ejecutar(reparacion);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }


        }
        [HttpGet("TodasLasReparaciones")]
        public async Task<ActionResult<ResponseReparacionesEnTallerDTO>> ObtenerTodasLasReparaciones()
        {
            try
            {
                var reparaciones = await obtenerTodasLasReparacionesUc.Ejecutar();
                IEnumerable<ReparacionEnTallerDTO> rep = reparaciones.Select(r => new ReparacionEnTallerDTO()
                {
                    Id = r.Id,
                    ClienteNombre = r.Cliente.Nombre,
                    ClienteApellido = r.Cliente.Apellido,
                    ClienteTelefono = r.Cliente.Telefono,
                    ClienteDireccion = r.Cliente.Direccion,
                    ClienteEmail = r.Cliente.Email.Value,
                    ClienteCedula=r.Cliente.Ci,
                    Producto = r.Producto,
                    NumeroSerie = r.NumeroSerie,
                    Descripcion = r.Descripcion,
                    Fecha = r.Fecha,
                    Estado=r.Estado,
                    DescripcionPresupuesto=r.DescripcionPresupuesto,
                    Costo=r.CostoFinal
                    
                    

                });
                ResponseReparacionesEnTallerDTO response = new ResponseReparacionesEnTallerDTO()
                {
                    StatusCode = 200,
                    reparaciones = rep.ToList()

                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                ResponseReparacionesEnTallerDTO response = new ResponseReparacionesEnTallerDTO()
                {
                    StatusCode = 500,
                    reparaciones = null

                };
                return BadRequest(response);
            }


        }

        

        

        
    }
}
