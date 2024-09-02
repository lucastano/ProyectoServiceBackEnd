using Microsoft.EntityFrameworkCore;
using ProyectoService.LogicaNegocio.IRepositorios;
using ProyectoService.LogicaNegocio.IServicios;
using ProyectoService.LogicaNegocio.Modelo;
using ProyectoService.LogicaNegocio.Servicios;
using ProyectoService.LogicaNegocio.Validaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoService.AccesoDatos.EntityFramework
{
    public class MensajeriaEFRepositorio : IMensajeriaRepositorio
    {
        ProyectoServiceContext _context;
        private readonly IUsuarioServicio usuarioServicio;
        private readonly IReparacionServicio reparacionServicio;
        public MensajeriaEFRepositorio(ProyectoServiceContext context,IUsuarioServicio usuarioServicio,IReparacionServicio reparacionServicio)
        {
            _context = context;
            this.reparacionServicio = reparacionServicio;
            this.usuarioServicio = usuarioServicio;
        }

        public async Task EliminarMensajesReparacion(int idReparacion)
        {
            if (idReparacion == 0) throw new Exception("Id incorrecto");
            List<Mensaje>mensajes= await ObtenerMensajesDeReparacion(idReparacion);
            if (mensajes.Count > 0)
            {
                _context.Mensajes.RemoveRange(mensajes);
                await _context.SaveChangesAsync();
            }
            
        }

        public async Task NuevoMensaje(Mensaje msg)
        {
           
            DateTime fechaHoraServidor = msg.FechaHoraEnvio;
            msg.FechaHoraEnvio = fechaHoraServidor.AddHours(-3);
            
            if (msg.DestinatarioId == msg.EmisorId) throw new Exception("Emisor y destinatario no pueden ser el mismo");
            if (msg == null) throw new Exception("Debe ingresar un mensaje");
            if (msg.Texto == null || msg.Texto==" ") throw new Exception("Falta el cuerpo del mensaje");
            if (msg.ReparacionId == 0) throw new Exception("falta asociar una reparacion");
            if (msg.DestinatarioId == 0) throw new Exception("Destinatario no asociado");
            if (msg.EmisorId == 0) throw new Exception("Emisor no asociado");

            Usuario usuarioEmisor= await usuarioServicio.ExisteUsuario(msg.EmisorId);
            Usuario usuarioDestinatario = await usuarioServicio.ExisteUsuario(msg.DestinatarioId);
            Reparacion reparacion = await reparacionServicio.ExisteReparacion(msg.ReparacionId);
            if (usuarioEmisor==null) throw new Exception("Emisor no existe");
            if (usuarioDestinatario==null) throw new Exception(" Destinatario no existe");
            if (reparacion==null) throw new Exception("Reparacion no existe");

            

            await _context.Mensajes.AddAsync(msg);
            await _context.SaveChangesAsync();

        }

        public async Task<List<Mensaje>> ObtenerMensajesDeReparacion(int idReparacion)
        {
            List<Mensaje>retorno = await _context.Mensajes.Include(m => m.Destinatario).Include(m => m.Emisor).Include(m => m.Reparacion).Where(m => m.ReparacionId == idReparacion).ToListAsync();
            return retorno;
        }
    }
}
