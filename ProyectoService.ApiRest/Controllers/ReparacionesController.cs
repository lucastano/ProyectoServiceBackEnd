using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoService.ApiRest.DTOs;
using ProyectoService.Aplicacion.CasosUso;
using ProyectoService.Aplicacion.ICasosUso;
using ProyectoService.LogicaNegocio.Modelo;

namespace ProyectoService.ApiRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class ReparacionesController : ControllerBase
    {
        private readonly IAgregarReparacion agregarReparacionUc;
        private readonly IObtenerTodasLasReparaciones obtenerTodasLasReparacionesUc;
        private readonly IObtenerReparacionPorId obtenerReparacionPorIdUc;
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
        private readonly IModificarPresupuestoReparacion modificarPresupuestoReparacionUc;
        private readonly IModificarDatosReparacion modificarDatosReparacionUc;
        private readonly IGenerarOrdenDeServicio generarOrdenDeServicioUc;
        private readonly IConfiguration configuration;
        private readonly IEliminarMensajesReparacion eliminarMensajesReparacionUc;
        private readonly IObtenerHistoriaClinica obtenerHistoriaClinicaUc;
        private readonly IObtenerMontoTotalHistoriaClinica obtenerMontoTotalHistoriaClinicaUc;
        private readonly IObtenerProductoPorId obtenerProductoPorIdUc;
        private readonly IObtenerReparacionesPorCliente obtenerReparacionesPorClienteUc;
        private Empresa emp;



        public ReparacionesController(IAgregarReparacion agregarReparacionUc, IObtenerTodasLasReparaciones obtenerTodasLasReparacionesUc,  IObtenerReparacionesPorTecnico obtenerReparacionesPorTecnicoUc, IPresupuestarReparacion presupuestarReparacionUc, IObtenerClientePorCI obtenerClientePorCiUc, IObtenerTecnicoPorId obtenerTecnicoPorIdUc, IAvisoNuevaReparacion avisoNuevaReparacionUc, IAvisoNuevoPresupuesto avisoNuevoPresupuestoUc, IAceptarPresupuesto aceptarPresupuestoUc, INoAceptarPresupuesto noAceptarPresupuestoUc, ITerminarReparacion terminarReparacionUc, IEntregarReparacion entregarReparacionUc, IAvisoEntregaReparacion avisoEntregarReparacionUc, IAvisoReparacionTerminada avisoReparacionTerminadaUc,  IConfiguration configuration, IObtenerReparacionPorId obtenerReparacionPorIdUc, IGenerarOrdenDeServicio generarOrdenDeServicioUc, IModificarPresupuestoReparacion modificarPresupuestoReparacionUc,IModificarDatosReparacion modificarDatosReparacionUc, IEliminarMensajesReparacion eliminarMensajesReparacionUc, IObtenerHistoriaClinica obtenerHistoriaClinicaUc, IObtenerMontoTotalHistoriaClinica obtenerMontoTotalHistoriaClinicaUc,IObtenerProductoPorId obtenerProductoPorIdUc, IObtenerReparacionesPorCliente obtenerReparacionesPorClienteUc)
        {
            this.agregarReparacionUc = agregarReparacionUc;
            this.obtenerTodasLasReparacionesUc = obtenerTodasLasReparacionesUc;
            this.obtenerReparacionPorIdUc = obtenerReparacionPorIdUc;
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
            this.modificarPresupuestoReparacionUc = modificarPresupuestoReparacionUc;
            this.modificarDatosReparacionUc = modificarDatosReparacionUc;
            this.eliminarMensajesReparacionUc = eliminarMensajesReparacionUc;
            this.obtenerMontoTotalHistoriaClinicaUc = obtenerMontoTotalHistoriaClinicaUc;
            this.obtenerHistoriaClinicaUc = obtenerHistoriaClinicaUc;
            this.obtenerProductoPorIdUc = obtenerProductoPorIdUc;
            this.obtenerReparacionesPorClienteUc = obtenerReparacionesPorClienteUc;
            this.configuration = configuration;
           
            var configNombreEmpresa = configuration.GetSection("EmpresaSettings:NombreEmpresa").Value!;
            var configDireccionEmpresa = configuration.GetSection("EmpresaSettings:DireccionEmpresa").Value!;
            var configTelefonoEmpresa = configuration.GetSection("EmpresaSettings:TelefonoEmpresa").Value!;
            var configEmail = configuration.GetSection("EmpresaSettings:Email").Value!;
            var configPassword = configuration.GetSection("EmpresaSettings:EmailPassword").Value!;
            var configPoliticasEmpresa = configuration.GetSection("EmpresaSettings:PoliticasEmpresa").Value!;
            Empresa empresaConfig = new Empresa()
            {
                Nombre = configNombreEmpresa,
                Direccion = configDireccionEmpresa,
                Telefono = configTelefonoEmpresa,
                Email = configEmail,
                EmailPassword = configPassword,
                PoliticasEmpresa= configPoliticasEmpresa
            };
            this.emp = empresaConfig;
            this.generarOrdenDeServicioUc = generarOrdenDeServicioUc;
        }

        [HttpPost]

        public async Task<ActionResult> NuevaReparacion(NuevaReparacionDTO dto)
        {
            if (!ModelState.IsValid) throw new Exception("Debe ingresar datos de la reparacion");
            try
            {
                
                
                Tecnico tecnico = await obtenerTecnicoPorIdUc.Ejecutar(dto.IdTecnico);
                if (tecnico == null) throw new Exception("Tecnico no existe");
                
                Cliente cliente = await obtenerClientePorCiUc.Ejecutar(dto.CiCliente);
                Reparacion reparacion = new Reparacion()
                {
                    Tecnico = tecnico,
                    Cliente = cliente,
                    Producto = await obtenerProductoPorIdUc.Ejecutar(dto.IdProducto),
                    NumeroSerie = dto.NumeroSerie,
                    Descripcion = dto.Descripcion,
                    FechaPromesaPresupuesto=dto.FechaPromesaPresupuesto

                };

               Reparacion rep= await agregarReparacionUc.Ejecutar(reparacion);
               byte []pdf=  await avisoNuevaReparacionUc.Ejecutar(rep);
               
                ResponseNuevaReparacionDTO response = new ResponseNuevaReparacionDTO()
                {
                    StatusCode = 200,
                    OrdenDeServicio=pdf
                };
                return Ok(response);




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
                await avisoNuevoPresupuestoUc.Ejecutar(rep); 
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
                byte[] pdf=await avisoEntregarReparacionUc.Ejecutar(reparacion);
                ResponseEntregarReparacionDTO response = new ResponseEntregarReparacionDTO()
                {
                    StatusCode = 200,
                    OrdenDeServicio=pdf

                };

                await eliminarMensajesReparacionUc.Ejecutar(reparacion.Id);


                return Ok(response);
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
                    TecnicoId = r.Tecnico.Id,
                    Producto = new ProductoDTO()
                    {
                        Id=r.Producto.Id,
                        Marca=r.Producto.Marca,
                        Modelo=r.Producto.Modelo,
                        Version=r.Producto.Version
                    },
                    NumeroSerie = r.NumeroSerie,
                    Descripcion = r.Descripcion,
                    Fecha = r.Fecha,
                    Estado=r.Estado,
                    DescripcionPresupuesto=r.DescripcionPresupuesto,
                    Costo=r.CostoFinal,
                    FechaPromesaPresupuesto=r.FechaPromesaPresupuesto,
                    FechaPromesaEntrega=r.FechaPromesaEntrega
                    
                    

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

        [HttpGet("ReparacionesDeClienteCedula")]

        public async Task<ActionResult> ObtenerReparacionesClientePorCedula(string cedula)
        {
            try
            {
                if (cedula == "") throw new Exception("Debe ingresar un email");
                List<Reparacion> reparacionesDeCliente = await obtenerReparacionesPorClienteUc.Ejecutar(cedula);
                if (!reparacionesDeCliente.Any())
                {
                    return Ok("No tiene reparaciones");

                }
                else
                {
                    IEnumerable<ReparacionEnTallerDTO> rep = reparacionesDeCliente.Select(r => new ReparacionEnTallerDTO()
                    {
                        Id = r.Id,
                        ClienteNombre = r.Cliente.Nombre,
                        ClienteApellido = r.Cliente.Apellido,
                        ClienteTelefono = r.Cliente.Telefono,
                        ClienteDireccion = r.Cliente.Direccion,
                        ClienteEmail = r.Cliente.Email.Value,
                        ClienteCedula = r.Cliente.Ci,
                        TecnicoId = r.Tecnico.Id,
                        Producto = new ProductoDTO()
                        {
                            Id = r.Producto.Id,
                            Marca = r.Producto.Marca,
                            Modelo = r.Producto.Modelo,
                            Version = r.Producto.Version
                        },
                        NumeroSerie = r.NumeroSerie,
                        Descripcion = r.Descripcion,
                        Fecha = r.Fecha,
                        Estado = r.Estado,
                        DescripcionPresupuesto = r.DescripcionPresupuesto,
                        Costo = r.CostoFinal,
                        FechaPromesaPresupuesto = r.FechaPromesaPresupuesto,
                        FechaPromesaEntrega = r.FechaPromesaEntrega



                    });
                    return Ok(rep);

                }

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("GenerarOrdenDeServicio")]
        public async Task<ActionResult>GenerarOrdenDeServicio(int id)
        {
            try
            {
                if (id == 0) throw new Exception("Numero de orden incorrecto");
                Reparacion rep = await obtenerReparacionPorIdUc.Ejecutar(id);
                byte[]pdf= generarOrdenDeServicioUc.Ejecutar(rep,emp);

                ResponseGenerarOrdenDeServicioDTO response = new ResponseGenerarOrdenDeServicioDTO()
                {
                    statusCode = 200,
                    OrdenDeServicio = pdf,
                };
                return Ok(response); 
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        [HttpPut("ModificarPresupuestoReparacion")]
        public async Task<ActionResult> ModificarPresupuestoReparacion(ModificarPresupuestoReparacionDTO dto)
        {
            try
            {
                if (dto.Id == 0) throw new Exception("Numero de orden de reparacion incorrecto");
                Reparacion rep =await modificarPresupuestoReparacionUc.Ejecutar(dto.Id, dto.Costo,dto.Descripcion);
                ReparacionEnTallerDTO reparacionDTO = new ReparacionEnTallerDTO()
                {
                    Id = rep.Id,
                    ClienteNombre = rep.Cliente.Nombre,
                    ClienteApellido = rep.Cliente.Apellido,
                    ClienteTelefono = rep.Cliente.Telefono,
                    ClienteDireccion = rep.Cliente.Direccion,
                    ClienteEmail = rep.Cliente.Email.Value,
                    ClienteCedula = rep.Cliente.Ci,
                    TecnicoId = rep.Tecnico.Id,
                    Producto = new ProductoDTO()
                    {
                        Id = rep.Producto.Id,
                        Marca = rep.Producto.Marca,
                        Modelo = rep.Producto.Modelo,
                        Version = rep.Producto.Version
                    },
                    NumeroSerie = rep.NumeroSerie,
                    Descripcion = rep.Descripcion,
                    Fecha = rep.Fecha,
                    Estado = rep.Estado,
                    DescripcionPresupuesto = rep.DescripcionPresupuesto,
                    Costo = rep.CostoFinal,
                    FechaPromesaPresupuesto = rep.FechaPromesaPresupuesto

                };
                return Ok(reparacionDTO);

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPut("ModificarDatosReparacion")]
        public async Task<ActionResult> ModificarDatosReparacion(ModificarDatosReparacionDTO dto)
        {
            try
            {
                if (dto.Id == 0) throw new Exception("Numero de orden incorrecto");
                Reparacion rep=await modificarDatosReparacionUc.Ejecutar(dto.Id,dto.FechaPromesaPresupuesto,dto.NumeroSerie,dto.Descripcion);
                ReparacionEnTallerDTO reparacionDTO = new ReparacionEnTallerDTO()
                {
                    Id = rep.Id,
                    ClienteNombre = rep.Cliente.Nombre,
                    ClienteApellido = rep.Cliente.Apellido,
                    ClienteTelefono = rep.Cliente.Telefono,
                    ClienteDireccion = rep.Cliente.Direccion,
                    ClienteEmail = rep.Cliente.Email.Value,
                    ClienteCedula = rep.Cliente.Ci,
                    TecnicoId = rep.Tecnico.Id,
                    Producto = new ProductoDTO()
                    {
                        Id = rep.Producto.Id,
                        Marca = rep.Producto.Marca,
                        Modelo = rep.Producto.Modelo,
                        Version = rep.Producto.Version
                    },
                    NumeroSerie = rep.NumeroSerie,
                    Descripcion = rep.Descripcion,
                    Fecha = rep.Fecha,
                    Estado = rep.Estado,
                    DescripcionPresupuesto = rep.DescripcionPresupuesto,
                    Costo = rep.CostoFinal,
                    FechaPromesaPresupuesto = rep.FechaPromesaPresupuesto

                };
                return Ok(reparacionDTO);

            }
            catch( Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("HistoriaClinica")]

        public async Task<ActionResult>ObtenerHistoriaClinica(string numeroSerie)
        {
            try
            {
                if (numeroSerie == null || numeroSerie.Length == 0) throw new Exception("Numero de serie incorrecto");
                List<Reparacion> reparaciones = await obtenerHistoriaClinicaUc.Ejecutar(numeroSerie);
                if (reparaciones.Count == 0) throw new Exception("Sin reparaciones realizadas");
                double montoTotal = await obtenerMontoTotalHistoriaClinicaUc.Ejecutar(numeroSerie);
                
                ResponseHistoriaClinicaDTO historiaClinica = new ResponseHistoriaClinicaDTO()
                {
                    CantidadReparacionesRealizadas = reparaciones.Count(),
                    GastoTotalEnReparaciones = montoTotal,
                    NumeroSerie = numeroSerie,
                    reparacionesRealizadas = reparaciones.Select(r => new ReparacionHCDTO()
                    {
                        FechaEntregaReparacion=r.FechaEntrega,
                        DescripcionProblema=r.Descripcion,
                        DescripcionSolucion=r.DescripcionPresupuesto,
                        CostoReparacion=r.CostoFinal
                    })
                     

                };
                return Ok(historiaClinica);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        

        
    }
}
