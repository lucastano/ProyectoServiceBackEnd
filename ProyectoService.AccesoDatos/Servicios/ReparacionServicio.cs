using Microsoft.EntityFrameworkCore;
using ProyectoService.LogicaNegocio.IServicios;
using ProyectoService.LogicaNegocio.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoService.AccesoDatos.Servicios
{
    public class ReparacionServicio : IReparacionServicio
    {
        private readonly ProyectoServiceContext _context;
        public ReparacionServicio(ProyectoServiceContext context)
        {
            _context = context;
        }

        public async Task<Reparacion> ExisteReparacion(int id)
        {
            Reparacion rep = await _context.Reparaciones.Include(r=>r.Cliente).Include(r=>r.Tecnico).FirstOrDefaultAsync(x => x.Id == id);
            return rep;
        }
    }
}
