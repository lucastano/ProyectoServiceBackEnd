using ProyectoService.LogicaNegocio.IRepositorios;
using ProyectoService.LogicaNegocio.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProyectoService.LogicaNegocio.Excepciones;

namespace ProyectoService.AccesoDatos.EntityFramework
{
    public class ClienteEFRepositorio : IClienteRepositorio
    {
        ProyectoServiceContext _context;
        
        public ClienteEFRepositorio(ProyectoServiceContext context)
        {
            _context = context;

        }
        public async Task Add(Cliente entity)
        {
            
          
                if (entity.Nombre == null) throw new ClienteException("Debe ingresar nombre de cliente");
                if (entity.Apellido == null) throw new ClienteException("Debe ingresar apellido de cliente");
                if (entity.Telefono == null) throw new ClienteException("Debe ingresar telefono de cliente");
                if (entity.Ci == null)  throw new ClienteException("cedula no valida"); 
                if (!entity.validarCi())  throw new ClienteException("Número de documento inválido"); 
                Cliente cliBuscado = await GetClienteByCi(entity.Ci);
                if (cliBuscado != null)  throw new ClienteException("Cliente ya existe"); 
                await _context.Clientes.AddAsync(entity);
                await _context.SaveChangesAsync();

        }

        public async Task Delete(Cliente entity)
        {
            //Eliminar cliente no se hasta donde es valido, ya que el cliente va a tener historicos de servicios 
            throw new NotImplementedException();
        }

        public async Task<List<Cliente>> getAll()
        {
            return  await _context.Clientes.ToListAsync();
            
        }

        //PUEDE DEVOLVER NULL
        public async Task<Cliente?> GetClienteByCi(string ci)
        {
            if (ci == null) throw new ClienteException("Debe ingresar una ci");
            
            return  await _context.Clientes.FirstOrDefaultAsync(c=>c.Ci==ci);
        }

        public async Task Update(Cliente entity)
        {
            throw new NotImplementedException();
        }
    }
}
