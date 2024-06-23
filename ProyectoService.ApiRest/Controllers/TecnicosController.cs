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
    
    public class TecnicosController : ControllerBase
    {
        private readonly IAgregarTecnico agregarTecnicoUc;
        private readonly IObtenerTodosLosTecnicos obtenerTodosLosTecnicosUc;
        private readonly IObtenerTecnicoPorEmail obtenerTecnicoPorEmailUc;
        private readonly IValidarPassword validarPasswordUc;

        public TecnicosController(IAgregarTecnico agregarTecnicoUc, IObtenerTodosLosTecnicos obtenerTodosLosTecnicosUc, IObtenerTecnicoPorEmail obtenerTecnicoPorEmailUc, IValidarPassword validarPasswordUc)
        {
            this.agregarTecnicoUc = agregarTecnicoUc;
            this.obtenerTodosLosTecnicosUc = obtenerTodosLosTecnicosUc;
            this.obtenerTecnicoPorEmailUc = obtenerTecnicoPorEmailUc;
            this.validarPasswordUc = validarPasswordUc;
        }

        [HttpPost]

        public async Task<ActionResult<ResponseAgregarTecnicoDTO>> AgregarTecnico(AgregarTecnicoDTO dto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(400);
            }
            

            if (!validarPasswordUc.Ejecutar(dto.Password))
            {
                return BadRequest("Password NO valido");
            }
            
            try
            {
                Seguridad.CrearPasswordHash(dto.Password, out byte[] passwordHash, out byte[] passwordSalt);
                Tecnico tecnico = new Tecnico()
                {
                    Nombre = dto.Nombre,
                    Apellido = dto.Apellido,
                    Email = EmailVO.Crear(dto.Email),
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt
                };
                await agregarTecnicoUc.Ejecutar(tecnico);
                ResponseAgregarTecnicoDTO response = new ResponseAgregarTecnicoDTO()
                {
                    statusCode = 201,
                    Tecnico = dto

                };

                return Ok(response);

            }
            catch (Exception ex)
            {
                return StatusCode(400,ex.Message);

            }

        }

        [HttpGet]

        public async  Task<ActionResult<ResponseObtenerTecnicosDTO>> ObtenerTecnicos()
        {
            try
            {
                var Tecnicos= await obtenerTodosLosTecnicosUc.Ejecutar();

                List<TecnicoDTO> tecnicos = Tecnicos.Select(t => new TecnicoDTO()
                {
                    Id= t.Id,
                    Nombre= t.Nombre,
                    Apellido= t.Apellido,
                    Email = t.Email.Value,
                    Rol= t.Rol

                }).ToList();

                ResponseObtenerTecnicosDTO response = new ResponseObtenerTecnicosDTO()
                {
                    StatusCode= 200,
                    Tecnicos = tecnicos

                };

                return Ok(response);

            }catch (Exception ex)
            {
                return StatusCode(500);
            }

        }
    }
}
