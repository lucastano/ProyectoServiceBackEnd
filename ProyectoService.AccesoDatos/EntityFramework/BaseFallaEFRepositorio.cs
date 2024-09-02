using Microsoft.EntityFrameworkCore;
using ProyectoService.LogicaNegocio.IRepositorios;
using ProyectoService.LogicaNegocio.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoService.AccesoDatos.EntityFramework
{
    public class BaseFallaEFRepositorio : IBaseFallaRepositorio
    {
        ProyectoServiceContext _context;

        public BaseFallaEFRepositorio(ProyectoServiceContext context)
        {
            _context = context;
        }

        public async Task Add(BaseFalla entity)
        {
            
                if (entity == null) throw new Exception("Debe ingresar una falla");
                if (entity.Producto == null) throw new Exception("Debe ingresar un producto");
                if (entity.Falla == "") throw new Exception("Debe ingresar una falla");
                if (entity.Solucion == "") throw new Exception("Debe ingresar una solucion");
                await _context.BaseFallas.AddAsync(entity);
                await _context.SaveChangesAsync();
            
        }

        public async Task Delete(BaseFalla entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<BaseFalla>> getAll()
        {
            return _context.BaseFallas.Include(f=>f.Producto).ToListAsync();

        }

        public async Task<List<BaseFalla>> ObtenerSegunDescripcion(string Descripcion)
        {
            return await _context.BaseFallas.Include(f=>f.Producto).Where(r => r.Falla.Contains(Descripcion.ToLower())).ToListAsync();
           
        }

        public Task Update(BaseFalla entity)
        {
            throw new NotImplementedException();
        }

        
    }
}
