using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoService.ApiRest.DTOs;
using ProyectoService.Aplicacion.ICasosUso;
using ProyectoService.LogicaNegocio.Modelo;
using ProyectoService.LogicaNegocio.Modelo.ValueObjects;

namespace ProyectoService.ApiRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class ClientesController : ControllerBase
    {
        private readonly IAgregarClienteUC agregarClienteUC;
        private readonly IObtenerTodosLosClientesUC obtenerClientesUC;
        private readonly IObtenerClientePorCI obtenerClientePorCiUC;
        public ClientesController(IAgregarClienteUC agregarClienteUC, IObtenerTodosLosClientesUC obtenerClientesUC, IObtenerClientePorCI obtenerClientePorCiUC)
        {
            this.agregarClienteUC = agregarClienteUC;
            this.obtenerClientesUC = obtenerClientesUC;
            this.obtenerClientePorCiUC = obtenerClientePorCiUC;
        }


        [HttpPost]

        public async Task<ActionResult<ResponseAgregarClienteDTO>> AltaCliente(AgregarClienteDTO dto)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest("Datos invalidos");
            }

            try
            {
                //passwordhash y salt no se crea con una password, ya que el cliente lo agrega el mismo tecnico, entonce se utiliza 
                // la cedula del cliente para autogenerar el passwordhash y salt
                //el cliente a futuro puede cambiar el password
                Seguridad.CrearPasswordHash(dto.Ci, out byte[] passwordHash, out byte[] passwordSalt);
                //TODO: ver controles necesarios en crear el cliente
                Cliente cli = new Cliente()
                {
                    Nombre=dto.Nombre,
                    Apellido=dto.Apellido,
                    Telefono=dto.Telefono,
                    Email=EmailVO.Crear(dto.Email),
                    Direccion=dto.Direccion,
                    Ci=dto.Ci,
                    PasswordHash=passwordHash,
                    PasswordSalt=passwordSalt

                };
                
               await agregarClienteUC.Ejecutar(cli);
                ResponseAgregarClienteDTO response = new ResponseAgregarClienteDTO()
                {
                    StatusCode = 201,
                    Cliente = dto
                };
                return StatusCode(201,response);


            }
            catch(Exception ex)
            {
                ResponseAgregarClienteDTO response = new ResponseAgregarClienteDTO()
                {
                    StatusCode =400,
                    Cliente = null,
                    Error=ex.Message

                };
                //return BadRequest(response);
                return BadRequest(response);
            }
            
        }
        [HttpGet]

        public async Task <ActionResult<ResponseGetClientesDTO>> GetClientes()
        {
            try
            {
                var clientes = await obtenerClientesUC.Ejecutar();
                IEnumerable<ClienteDTO> cli = clientes.Select(c => new ClienteDTO()
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Apellido = c.Apellido,
                    Telefono = c.Telefono,
                    Direccion = c.Direccion,
                    Email = c.Email.Value,
                    Rol=c.Rol,
                    Ci = c.Ci

                });
                ResponseGetClientesDTO response = new ResponseGetClientesDTO()
                {
                   StatusCode = 200,
                   Clientes=cli.ToList(),

                };
                return response;
            }
            catch (Exception ex)
            {
                ResponseGetClientesDTO response = new ResponseGetClientesDTO()
                {
                    StatusCode = 500,
                    Clientes = null

                };
                return BadRequest(response);
            }


        }

        [HttpGet("ObtenerClientePorCi")]

        public async Task<ActionResult<ResponseObtenerClientePorCiDTO>> ObtenerClientePorCi(string ci)
        {
            try
            {
                Cliente cliente = await obtenerClientePorCiUC.Ejecutar(ci);
                ClienteDTO clienteDTO = new ClienteDTO()
                {
                    Id = cliente.Id,
                    Nombre = cliente.Nombre,
                    Apellido = cliente.Apellido,
                    Telefono = cliente.Telefono,
                    Direccion = cliente.Direccion,
                    Email = cliente.Email.Value,
                    Rol = cliente.Rol,
                    Ci = cliente.Ci

                };
               
                ResponseObtenerClientePorCiDTO response = new ResponseObtenerClientePorCiDTO()
                {
                    StatusCode = 200,
                    cliente = clienteDTO,
                    Error=""

                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                ResponseObtenerClientePorCiDTO response = new ResponseObtenerClientePorCiDTO()
                {
                    StatusCode = 500,
                    cliente = null,
                    Error=ex.Message

                };
                return BadRequest(response);
            }


        }
    }
}
