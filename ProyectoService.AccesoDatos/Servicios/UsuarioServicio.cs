using Microsoft.EntityFrameworkCore;
using ProyectoService.LogicaNegocio.Modelo;
using ProyectoService.LogicaNegocio.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoService.AccesoDatos.Servicios
{
    public class UsuarioServicio : IUsuarioServicio
    {
        private readonly ProyectoServiceContext _context;
        public UsuarioServicio(ProyectoServiceContext context)
        {
            _context = context;
        }

        public async Task<Usuario> ExisteUsuario(int id)
        {
            Usuario usuario =await  _context.Tecnicos.FirstOrDefaultAsync(t => t.Id == id);
            if(usuario == null)
            {
                usuario = await _context.Clientes.FirstOrDefaultAsync(t => t.Id == id);
            }
            if (usuario == null)
            {
                usuario = await _context.Administradores.FirstOrDefaultAsync(t => t.Id == id);

            }

            return usuario;
        }
    }
}
