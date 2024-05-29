﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoService.ApiRest.DTOs;
using ProyectoService.Aplicacion.ICasosUso;
using ProyectoService.LogicaNegocio.Modelo;

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

        public ActionResult<ResponseAgregarTecnicoDTO> AgregarTecnico(AgregarTecnicoDTO dto)
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
                    Email = dto.Email,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt
                };
                agregarTecnicoUc.Ejecutar(tecnico);
                ResponseAgregarTecnicoDTO response = new ResponseAgregarTecnicoDTO()
                {
                    statusCode = 201,
                    Tecnico = dto

                };

                return response;

            }
            catch (Exception ex)
            {
                return StatusCode(500);

            }

        }

        [HttpGet]

        public ActionResult<ResponseObtenerTecnicosDTO> ObtenerTecnicos()
        {
            try
            {
                var Tecnicos =obtenerTodosLosTecnicosUc.Ejecutar();

                List<TecnicoDTO> tecnicos = Tecnicos.Select(t => new TecnicoDTO()
                {
                    Id= t.Id,
                    Nombre= t.Nombre,
                    Apellido= t.Apellido,
                    Email = t.Email,
                    Rol= t.Rol

                }).ToList();

                ResponseObtenerTecnicosDTO response = new ResponseObtenerTecnicosDTO()
                {
                    StatusCode= 200,
                    Tecnicos = tecnicos

                };

                return response;

            }catch (Exception ex)
            {
                return StatusCode(500);
            }

        }
    }
}