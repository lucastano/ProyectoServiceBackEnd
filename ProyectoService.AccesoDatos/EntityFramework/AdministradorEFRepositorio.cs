using Microsoft.EntityFrameworkCore;
using ProyectoService.LogicaNegocio.Excepciones;
using ProyectoService.LogicaNegocio.IRepositorios;
using ProyectoService.LogicaNegocio.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoService.AccesoDatos.EntityFramework
{
    public class AdministradorEFRepositorio : IAdministradorRepositorio
    {
        ProyectoServiceContext _context;

        public AdministradorEFRepositorio(ProyectoServiceContext context)
        {
            _context = context;
        }

        public async Task Add(Administrador entity)
        {
            if (entity == null) throw new AdministradorException("Administrador esta vacio");
            if (entity.Nombre == null) throw new AdministradorException("Debe ingresar nombre del administrador");
            if (entity.Apellido == null) throw new AdministradorException("Debe ingresar apellido del administrador");
            
            Administrador adminBuscado = await ObtenerAdministradorPorEmail(entity.Email.Value);
            if (adminBuscado != null) throw new AdministradorException("Administrador ya existe");
            await _context.Administradores.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public Task Delete(Administrador entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Administrador>> getAll()
        {
            return await _context.Administradores.ToListAsync();
        }

        public async Task<Administrador> ObtenerAdministradorPorEmail(string email)
        {
            if (email == null) throw new AdministradorException("Debe ingresar un email");
            var administradores =await _context.Administradores.ToListAsync();
            return administradores.FirstOrDefault(a => a.Email.Value.Equals(email));
        }

        public Task Update(Administrador entity)
        {
            throw new NotImplementedException();
        }
    }
}
