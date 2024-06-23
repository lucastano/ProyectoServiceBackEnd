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
    public class EmpresaEFRepositorio : IEmpresaRepositorio
    {
        ProyectoServiceContext _context;
        public EmpresaEFRepositorio(ProyectoServiceContext context)
        {
            _context = context;
        }

        public async Task Add(Empresa entity)
        {
            List<Empresa> list =await getAll();
            if (list.Count > 0) throw new Exception("Solo puede existir una empresa");
            if (entity.Nombre == null) throw new Exception("Debe ingresar nombre de la empresa");
            //aca van mas validaciones
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public Task Delete(Empresa entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Empresa>> getAll()
        {
            return await _context.Empresas.ToListAsync();

        }

        public Task<Empresa> GetEmpresa()
        {
            throw new NotImplementedException();
        }

        public Task Update(Empresa entity)
        {
            throw new NotImplementedException();
        }
    }
}
