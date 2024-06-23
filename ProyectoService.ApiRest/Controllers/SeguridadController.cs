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
    
    public class SeguridadController : ControllerBase
    {
        private readonly IObtenerUsuario ucObtenerUsuarioCU;
        private readonly IConfiguration configuration;
        public SeguridadController(IObtenerUsuario ucObtenerUsuarioCU, IConfiguration configuration)
        {
            this.ucObtenerUsuarioCU = ucObtenerUsuarioCU;
            this.configuration = configuration;
        }


        [HttpPost]

        public async Task<ActionResult<ResponseLoginDTO>> Login(LoginDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(400);
            }
            try
            {
                Usuario usuarioModel = await ucObtenerUsuarioCU.Ejecutar(dto.Email,dto.rol);
                
                if (!Seguridad.VerificarPasswordHash(dto.Password, usuarioModel.PasswordHash, usuarioModel.PasswordSalt))
                {
                    return BadRequest("Password incorrecto");
                }
                string token = Seguridad.CrearToken(usuarioModel, configuration);
                UsuarioLogeadoDTO usuarioLogeado = new UsuarioLogeadoDTO();
                if (usuarioModel is Cliente)
                {
                    Cliente cliente= (Cliente)usuarioModel;
                    usuarioLogeado.Id= cliente.Id;
                    usuarioLogeado.Nombre= cliente.Nombre;
                    usuarioLogeado.Apellido= cliente.Apellido;
                    usuarioLogeado.Direccion = cliente.Direccion;
                    usuarioLogeado.Telefono= cliente.Telefono;
                    usuarioLogeado.Ci=cliente.Ci;
                    usuarioLogeado.Email= cliente.Email.Value;
                    usuarioLogeado.Rol = cliente.Rol;



                }
                else if (usuarioModel is Tecnico)
                {
                    Tecnico tecnico =(Tecnico)usuarioModel;
                    usuarioLogeado.Id = tecnico.Id;
                    usuarioLogeado.Nombre = tecnico.Nombre;
                    usuarioLogeado.Apellido = tecnico.Apellido;
                    usuarioLogeado.Email = tecnico.Email.Value;
                    usuarioLogeado.Rol = tecnico.Rol;

                }
                else
                {
                    Administrador administrador = (Administrador)usuarioModel;
                    usuarioLogeado.Id = administrador.Id;
                    usuarioLogeado.Nombre = administrador.Nombre;
                    usuarioLogeado.Apellido = administrador.Apellido;
                    usuarioLogeado.Email = administrador.Email.Value;
                    usuarioLogeado.Rol = administrador.Rol;

                }

                ResponseLoginDTO response = new ResponseLoginDTO()
                {
                    StatusCode = 200,
                    Token = token,
                    Usuario = usuarioLogeado,
                    Error=""
                };

                return Ok(response);

               
            }
            catch(Exception ex)
            {
                ResponseLoginDTO response = new ResponseLoginDTO()
                {
                    StatusCode = 400,
                    Token = null,
                    Usuario = null,
                    Error = ex.Message
                };
                return BadRequest(response);

            }

        }
    }
}
