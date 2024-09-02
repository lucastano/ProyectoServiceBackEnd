using Microsoft.EntityFrameworkCore;
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
    public class ProductoRepositorio : IProductoRepositorio
    {
        ProyectoServiceContext _context;
        public ProductoRepositorio(ProyectoServiceContext context)
        {
            _context = context;
        }

        public async Task Add(Producto entity)
        {
            if (entity == null) throw new Exception("Debe ingresar producto");
            if (entity.Marca == null ||entity.Marca=="") throw new Exception("Debe ingresar marca");
            entity.Marca=ValidacionesTexto.FormatearTexto(entity.Marca);
            if (entity.Modelo == null || entity.Modelo == "") throw new Exception("Debe ingresar modelo");
            entity.Modelo = ValidacionesTexto.FormatearTexto(entity.Modelo);
            if (entity.Version == null || entity.Version == "") throw new Exception("Debe ingresar Version");
            entity.Version=ValidacionesTexto.FormatearTexto(entity.Version);
            await _context.Productos.AddAsync(entity);
            await _context.SaveChangesAsync();

        }

        public Task Delete(Producto entity)
        {
            
            throw new NotImplementedException();
        }

        public async Task<List<Producto>> getAll()
        {
            return await _context.Productos.ToListAsync();
        }

        public async Task<Producto> ObtenerProductoPorId(int id)
        {
            return await _context.Productos.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task Update(Producto entity)
        {
            if (entity == null) throw new Exception("Se debe ingresar una reparacion a modificar");
            if (entity.Marca == null || entity.Marca == "") throw new Exception("Debe ingresar marca");
            if (entity.Modelo == null || entity.Modelo == "") throw new Exception("Debe ingresar modelo");
            if (entity.Version == null || entity.Version == "") throw new Exception("Debe ingresar Version");
            Producto producto=await ObtenerProductoPorId(entity.Id);
            if (producto == null) throw new Exception("Producto no existe");
             _context.Productos.Remove(producto);
             _context.SaveChanges();
        }
    }
}
