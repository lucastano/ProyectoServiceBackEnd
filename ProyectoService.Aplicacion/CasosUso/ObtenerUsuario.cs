﻿using ProyectoService.Aplicacion.ICasosUso;
using ProyectoService.LogicaNegocio.IRepositorios;
using ProyectoService.LogicaNegocio.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoService.Aplicacion.CasosUso
{
    public class ObtenerUsuario : IObtenerUsuario
    {
        private readonly IAuthService _authService;
        public ObtenerUsuario(IAuthService authService)
        {
            _authService = authService;
        }

        public Usuario Ejecutar(string email)
        {
            return _authService.Login(email);
        }
    }
}
