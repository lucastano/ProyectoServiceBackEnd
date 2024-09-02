using Microsoft.EntityFrameworkCore;
using ProyectoService.LogicaNegocio.Excepciones;
using ProyectoService.LogicaNegocio.IRepositorios;
using ProyectoService.LogicaNegocio.Modelo;
using ProyectoService.LogicaNegocio.Validaciones;
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
            entity.Nombre=ValidacionesTexto.FormatearTexto(entity.Nombre);
            entity.Apellido = ValidacionesTexto.FormatearTexto(entity.Apellido);
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

        public async Task<Administrador> ObtenerAdministradorPorEmail(string emailRecibido)
        {
            string email = emailRecibido.ToLower();
            if (email == null) throw new AdministradorException("Debe ingresar un email");
            var administradores =await _context.Administradores.ToListAsync();
            return administradores.FirstOrDefault(a => a.Email.Value.Equals(email));
        }

        public async Task<bool> CambiarPassword(string emailRecibido ,byte[]passwordHash,byte[]passwordSalt)
        {
            try
            {
                string email=emailRecibido.ToLower();
                if (email == "") throw new AdministradorException("Debe ingresar un email");
                if (passwordHash == null || passwordHash.Length == 0) throw new AdministradorException("Password no valido");
                if (passwordSalt == null || passwordSalt.Length == 0) throw new AdministradorException("Password no valido");
                Administrador admin = await ObtenerAdministradorPorEmail(email);
                if (admin == null) throw new AdministradorException("No existe administrador con ese email");
                admin.PasswordHash = passwordHash;
                admin.PasswordSalt = passwordSalt;
                await _context.SaveChangesAsync(); 
                return true;

            }
            catch (Exception ex)
            {
                return false;

            }


        }

        public Task Update(Administrador entity)
        {
            throw new NotImplementedException();
        }
    }
}
