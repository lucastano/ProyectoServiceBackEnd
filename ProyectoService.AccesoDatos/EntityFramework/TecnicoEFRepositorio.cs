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
            Tecnico tecnicoBuscado= await ObtenerTecnicoPorEmail(entity.Email.Value);
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

        public async Task<Tecnico?> ObtenerTecnicoPorEmail(string email)
        {
            if (email == null) throw new TecnicoException("Debe ingresar un email");
            //lo hice de esta forma porque daba error el equals en una query 
            var tecnicos = await _context.Tecnicos.ToListAsync();
            return tecnicos.FirstOrDefault(t=>t.Email.Value.Equals(email));

        }

        public async Task<Tecnico> ObtenerTecnicoPorId(int id)
        {
            var tecnicos= await _context.Tecnicos.ToListAsync();

            return tecnicos.FirstOrDefault(t => t.Id == id);
        }

        public async Task Update(Tecnico entity)
        {
            throw new NotImplementedException();
        }
    }
}
