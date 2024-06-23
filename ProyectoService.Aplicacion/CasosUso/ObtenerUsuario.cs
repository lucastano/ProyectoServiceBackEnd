using ProyectoService.Aplicacion.ICasosUso;
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

        public async Task<Usuario> Ejecutar(string email,string rol)
        {
            return await _authService.Login(email, rol);
        }
    }
}
