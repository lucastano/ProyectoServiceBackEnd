using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoService.ApiRest.DTOs;
using ProyectoService.Aplicacion.CasosUso;
using ProyectoService.Aplicacion.ICasosUso;
using ProyectoService.LogicaNegocio.Modelo;
using ProyectoService.LogicaNegocio.Modelo.ValueObjects;

namespace ProyectoService.ApiRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministradoresController : ControllerBase
    {
        private readonly IAgregarAdministrador agregarAdministradorUc;
        private readonly IObtenerAdministradores obtenerTodosLosAdministradoresUc;
        private readonly IValidarPassword validarPasswordUc;    

        public AdministradoresController(IAgregarAdministrador agregarAdministradorUc, IObtenerAdministradores obtenerTodosLosAdministradoresUc, IValidarPassword validarPasswordUc)
        {
            this.agregarAdministradorUc = agregarAdministradorUc;
            this.obtenerTodosLosAdministradoresUc = obtenerTodosLosAdministradoresUc;
            this.validarPasswordUc = validarPasswordUc;
        }

        [HttpPost]

        public async Task<ActionResult> AgregarAdministrador(AgregarAdministradorDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Debe ingresar todos los campos ");
            }
            try
            {
                
                if (!validarPasswordUc.Ejecutar(dto.Password)) throw new Exception("Password no valido");

                Seguridad.CrearPasswordHash(dto.Password, out byte[] PasswordHash, out byte[] PasswordSalt);
                Administrador admin = new Administrador()
                {
                    Nombre = dto.Nombre,
                    Apellido = dto.Apellido,
                    Email = EmailVO.Crear(dto.Email),
                    PasswordHash= PasswordHash,
                    PasswordSalt= PasswordSalt


                };

                await agregarAdministradorUc.Ejecutar(admin);

                ResponseAgregarAdministradorDTO response = new ResponseAgregarAdministradorDTO()
                {
                    StatusCode = 201,
                    administradorDTO=dto,
                    Error=""
                    
                };
                return Ok(response);

            }
            catch (Exception ex)
            {
                ResponseAgregarAdministradorDTO response = new ResponseAgregarAdministradorDTO()
                {
                    StatusCode = 400,
                    administradorDTO = null,
                    Error = ex.Message

                };

                return BadRequest(response);

            }

            


        }

        [HttpGet]

        public async Task<ActionResult<ResponseObtenerAdministradoresDTO>> ObtenerAdministradores()
        {
            try
            {
                var administradores = await obtenerTodosLosAdministradoresUc.Ejecutar();

                List<AdministradorDTO> Administradores = administradores.Select(a => new AdministradorDTO()
                {
                    Id = a.Id,
                    Nombre = a.Nombre,
                    Apellido = a.Apellido,
                    Email = a.Email.Value,
                  

                }).ToList();

                ResponseObtenerAdministradoresDTO response = new ResponseObtenerAdministradoresDTO()
                {
                    StatusCode = 200,
                    administradores = Administradores,
                    Error=""
                    

                };

                return Ok(response);

            }
            catch (Exception ex)
            {
                ResponseObtenerAdministradoresDTO response = new ResponseObtenerAdministradoresDTO()
                {
                    StatusCode = 500,
                    administradores = null,
                    Error = ex.Message

                };



                return StatusCode(500, response);
            }

        }
    }
}
