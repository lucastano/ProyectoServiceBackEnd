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
    public class TecnicoEFRepositorio : ITecnicoRepositorio
    {
        ProyectoServiceContext _context;
        public TecnicoEFRepositorio(ProyectoServiceContext context)
        {
            _context = context;
        }

        public async Task Add(Tecnico entity)
        {
            if (entity == null) throw new Exception("No ingreso los datos del tecnico");
            if (entity.Nombre == null) throw new TecnicoException("Debe ingresar nombre del tecnico");
            if (entity.Apellido == null) throw new TecnicoException("Debe ingresar apellido del tecnico");
            if (entity.Email.Value == null) throw new TecnicoException("Debe ingresar email del tecnico");
            entity.Nombre=ValidacionesTexto.FormatearTexto(entity.Nombre);
            entity.Apellido=ValidacionesTexto.FormatearTexto(entity.Apellido);
            Tecnico? tecnicoBuscado= await ObtenerTecnicoPorEmail(entity.Email.Value);
            if (tecnicoBuscado != null) throw new TecnicoException("Ya existe este tecnico");   
           await _context.Tecnicos.AddAsync(entity);
           await _context.SaveChangesAsync();
        }

       

        public async Task Delete(Tecnico entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Tecnico>> getAll()
        {
             return await _context.Tecnicos.ToListAsync();
        }

        public async Task<Tecnico?> ObtenerTecnicoPorEmail(string emailRecibido)
        {
            string email = emailRecibido.ToLower();
            if (email == null) throw new TecnicoException("Debe ingresar un email");
          
            var tecnicos = await _context.Tecnicos.ToListAsync();
            return tecnicos.FirstOrDefault(t=>t.Email.Value.Equals(email));

        }

        public async Task<Tecnico?> ObtenerTecnicoPorId(int id)
        {
            var tecnicos= await _context.Tecnicos.ToListAsync();
            return tecnicos.FirstOrDefault(t => t.Id == id);
        }


      
        public async Task<bool> CambiarPassword(string emailRecibido,byte[]passwordHash,byte[]passwordSalt)
        {
            try
            {
                string email =emailRecibido.ToLower();
                if (email == "") throw new TecnicoException("Falta email");
                if (passwordHash==null || passwordHash.Length==0) throw new TecnicoException("Contraseña no puede ser vacia");
                if (passwordSalt == null|| passwordSalt.Length==0) throw new TecnicoException("Contraseña no puede ser vacia");
                Tecnico? tecnicoBuscado = await ObtenerTecnicoPorEmail(email);
                if (tecnicoBuscado == null) throw new TecnicoException("No existe un tecnico con ese email");
                tecnicoBuscado.PasswordHash = passwordHash;
                tecnicoBuscado.PasswordSalt = passwordSalt;
                await _context.SaveChangesAsync();
                return true;

            }
            catch(Exception ex)
            {
                return false;
            }
            
            
        }

        public async Task Update(Tecnico entity)
        {
           if(entity.Nombre==null)throw new TecnicoException("Debe ingresar nombre");
           if (entity.Apellido == null) throw new TecnicoException("Debe ingresar apellido");
           if (entity.Email.Value == null) throw new TecnicoException("Debe ingresar email");
           entity.Nombre=ValidacionesTexto.FormatearTexto(entity.Nombre);
           entity.Apellido = ValidacionesTexto.FormatearTexto(entity.Apellido);
           await _context.SaveChangesAsync();
        }
    }
}
