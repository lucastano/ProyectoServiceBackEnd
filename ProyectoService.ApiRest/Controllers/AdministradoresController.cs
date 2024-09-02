using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoService.ApiRest.DTOs;
using ProyectoService.Aplicacion.CasosUso;
using ProyectoService.Aplicacion.ICasosUso;
using ProyectoService.LogicaNegocio.Modelo;
using ProyectoService.LogicaNegocio.Modelo.ValueObjects;
using System.Runtime.Serialization;

namespace ProyectoService.ApiRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class AdministradoresController : ControllerBase
    {
        private readonly IAgregarAdministrador agregarAdministradorUc;
        private readonly IObtenerAdministradores obtenerTodosLosAdministradoresUc;
        private readonly IValidarPassword validarPasswordUc; 
        private readonly ICambiarPasswordAdministrador cambiarPasswordUc;
        private readonly IObtenerAdministradorPorEmail obtenerAdministradorPorEmailUc;
        private readonly IAvisoCambioPassword avisoCambioPasswordUc;

        public AdministradoresController(IAgregarAdministrador agregarAdministradorUc, IObtenerAdministradores obtenerTodosLosAdministradoresUc, IValidarPassword validarPasswordUc, ICambiarPasswordAdministrador cambiarPasswordUc, IObtenerAdministradorPorEmail obtenerAdministradorPorEmailUc, IAvisoCambioPassword avisoCambioPasswordUc)
        {
            this.agregarAdministradorUc = agregarAdministradorUc;
            this.obtenerTodosLosAdministradoresUc = obtenerTodosLosAdministradoresUc;
            this.validarPasswordUc = validarPasswordUc;
            this.cambiarPasswordUc = cambiarPasswordUc;
            this.obtenerAdministradorPorEmailUc = obtenerAdministradorPorEmailUc;
            this.avisoCambioPasswordUc = avisoCambioPasswordUc;

        }
        
        [HttpPost]
        public async Task<ActionResult> AgregarAdministrador(AgregarAdministradorDTO dto)
        {
            
            try
            {
                if (!ModelState.IsValid) throw new Exception("Debe llenar todos los campos");
                if (!validarPasswordUc.Ejecutar(dto.Password)) throw new Exception("Contraseña no valida");
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
        [Authorize]
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

        [HttpPut("RecuperarPassword")]
        public async Task<ActionResult>RecuperarPassword(string email)
        {
            try
            {
                Administrador adminBuscado = await obtenerAdministradorPorEmailUc.Ejecutar(email);
                if (adminBuscado == null) throw new Exception("No existe administrador con ese email");
                string passRandom = Seguridad.GenerarPasswordRandom();
                Seguridad.CrearPasswordHash(passRandom,out byte[]passwordHash,out byte[]passwordSalt);
                bool resultado =await  cambiarPasswordUc.Ejecutar(email, passwordHash, passwordSalt);
                if (!resultado) throw new Exception("No existe administrador con ese email");
                await avisoCambioPasswordUc.Ejecutar(adminBuscado,passRandom);
                return Ok();
           


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }
        [Authorize]
        [HttpPut("CambiarPassword")]

        public async Task<ActionResult> CambiarPassword(string email,string password)

        {
           
            try
            {
                Administrador adminBuscado = await obtenerAdministradorPorEmailUc.Ejecutar(email);
                if (adminBuscado == null) throw new Exception("No existe administrador con ese email");
                if (!validarPasswordUc.Ejecutar(password)) throw new Exception("formato de la contraseña invalido");
                Seguridad.CrearPasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
                bool resultado = await cambiarPasswordUc.Ejecutar(email, passwordHash, passwordSalt);
                if (!resultado) throw new Exception("No existe administrador con ese email"); 
                return Ok();



            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }
    }
}
