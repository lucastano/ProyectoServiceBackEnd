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
    public class ReparacionEFRepositorio : IReparacionRepositorio
    {
        ProyectoServiceContext _context;
        
        public ReparacionEFRepositorio(ProyectoServiceContext context)
        {
            _context = context;
           
            
        }
       
        public async Task Add(Reparacion entity)
        {
            if (entity == null) throw new ReparacionException("Debe ingresar una reparacion");
            if (entity.Cliente == null) throw new ReparacionException("Debe ingresar un cliente");
            if (entity.Descripcion == null) throw new ReparacionException("Debe ingresar una descripcion");
            if (entity.Producto == null) throw new ReparacionException("Debe ingresar un producto");
            if (entity.NumeroSerie == null) throw new ReparacionException("Debe ingresar numero de serie");
            await _context.Reparaciones.AddAsync(entity);
            await _context.SaveChangesAsync();
            
        }

        public async Task<Reparacion>AddAlternativo(Reparacion entity)
        {
           
                if (entity == null) throw new ReparacionException("Debe ingresar una reparacion");
                if (entity.Tecnico == null) throw new ReparacionException("Debe ingresar un tecnico");
                if (entity.Cliente == null) throw new ReparacionException("Debe ingresar un cliente");
                if (entity.Descripcion == null) throw new ReparacionException("Debe ingresar una descripcion");
                if (entity.Producto == null) throw new ReparacionException("Debe ingresar un producto");
                if (entity.FechaPromesaPresupuesto == DateTime.MinValue) throw new ReparacionException("Debe ingresar una fecha aproximada para el presupuesto");
                if (entity.NumeroSerie == null) throw new ReparacionException("Debe ingresar numero de serie");
                await _context.Reparaciones.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            
            
        }

        

        public Task Delete(Reparacion entity)
        {
            throw new NotImplementedException();
        }

        //todas las reparaciones
        public async Task<List<Reparacion>> getAll()
        {
            return await _context.Reparaciones.Include(r=>r.Tecnico).Include(r=>r.Cliente).Include(r=>r.Producto ).ToListAsync();
        }

        public async Task<Reparacion> Presupuestar(int id, double ManoObra, string Descripcion,DateTime fechaPromesaEntrega)
        {
            Reparacion reparacion = await ObtenerReparacionPorId(id);
            if (reparacion == null) throw new ReparacionException("Reparacion no existe");
            if (reparacion.Estado != "EnTaller") throw new ReparacionException("Esta reparacion ya fue presupuestada");
            if (Descripcion == null) throw new ReparacionException("Debe ingresar una descripcion");
            if (ManoObra == 0) throw new ReparacionException("Debe ingresar un valor de mano de obra"); 
            reparacion.Presupuestar(ManoObra, Descripcion,fechaPromesaEntrega);
            await _context.SaveChangesAsync();
            return reparacion;

        }
        public async Task<Reparacion> Entregar(int id)
        {
          
            Reparacion reparacion = await ObtenerReparacionPorId(id);
            if (reparacion == null) throw new ReparacionException("reparacion no existe");
            if (reparacion.Estado != "Terminada") throw new ReparacionException("Esta reparacion aun no esta terminada");
            reparacion.Entregar();
            await _context.SaveChangesAsync();
            return reparacion;
        }

      
        public async Task AceptarPresupuesto(int id)
        {
            
            Reparacion reparacion = await ObtenerReparacionPorId(id);
            if (reparacion == null) throw new ReparacionException("Reparacion no existe");
            if (reparacion.Estado != "Presupuestada") throw new ReparacionException("Esta reparacion aun no esta presupuestada");
            
            reparacion.AceptarPresupuesto();
            await _context.SaveChangesAsync();
            
        }

        

        public async Task NoAceptarPresupuesto(int id, double costo,string razon)
        {
            
            Reparacion reparacion = await ObtenerReparacionPorId(id);
            if (reparacion == null) throw new ReparacionException("Reparacion no existe");
            if (reparacion.Estado !="Presupuestada") throw new ReparacionException("Esta reparacion aun no esta presupuestada");
            reparacion.NoAceptarPresupuesto(costo,razon);
            await _context.SaveChangesAsync();
            
        }

        public async Task<Reparacion> Terminar(int id, bool reparada)
        {
            
            
            Reparacion reparacion = await ObtenerReparacionPorId(id);
            if (reparacion == null) throw new ReparacionException("Reparacion no existe");
            if(reparacion.Estado=="EnTaller" || reparacion.Estado == "Presupuestada") throw new ReparacionException("Esta reparacion aun no se puede terminar");
            if (reparacion.Estado == "Terminada") throw new ReparacionException("Esta reparacion ya fue terminada");
            if (reparacion.Estado == "Entregada") throw new ReparacionException("Esta reparacion ya fue entregada");
            reparacion.Terminar(reparada);
            await _context.SaveChangesAsync();
            return reparacion;
            
        }




        
        public async Task<List<Reparacion>> ObtenerReparacionesPorCliente(string Ci)
        {
            
            List<Reparacion> reparaciones = await getAll();
            return reparaciones.Where(r => r.Cliente.Ci == Ci).ToList();
        }

       
        public async Task<List<Reparacion>> ObtenerReparacionesPorTecnico(string EmailTecnico)
        {
            
            List<Reparacion> reparaciones = await getAll();
            return reparaciones.Where(r=>r.Tecnico.Email.Value.Equals(EmailTecnico)).ToList();
        }

        
        public async Task<Reparacion> ObtenerReparacionPorId(int id)
        {
            var reparaciones = await getAll();
            return reparaciones.FirstOrDefault(r => r.Id == id);
        }

        public async Task Update(Reparacion entity)
        {
           
            throw new NotImplementedException();
        }

        public byte[] GenerarOrdenDeServicio(Reparacion rep,Empresa emp)
        {
            if (rep == null) throw new ReparacionException("Reparaicon no existe");
            byte[] pdf= rep.GenerarOrdenDeServicio(emp);
            return pdf;
        }
       
        public async Task<Reparacion> ModificarPresupuestoReparacion(int id, double costo, string descripcion)
        {
            Reparacion rep = await ObtenerReparacionPorId(id);
            if (rep == null) throw new ReparacionException("No existe la reparacion");
            if (rep.Estado == "Entregada") throw new ReparacionException("Esta reparacion ya fue entregada");
            if (rep.Estado == "EnTaller") throw new ReparacionException("Esta reparacion aun no esta presupuestada");
            if (descripcion != string.Empty)
            {
                rep.DescripcionPresupuesto = descripcion;
            }
            if (costo != 0)
            {
                rep.ManoDeObra = costo;
                rep.CostoFinal = costo;

            }
            
            await _context.SaveChangesAsync();
            return rep;
        }
        public async Task<Reparacion> ModificarDatosReparacion(int id ,DateTime fechaPromesaPresupuesto, string numeroSerie,string descripcion)
        {
            Reparacion reparacion = await ObtenerReparacionPorId(id);
            if (reparacion == null)throw new ReparacionException("Esta reparacion no existe");
            if (reparacion.Estado == "Entregada") throw new ReparacionException("Esta reparacion ya fue entregada");
            if(fechaPromesaPresupuesto!=DateTime.MinValue && fechaPromesaPresupuesto != DateTime.MaxValue)
            {

                reparacion.FechaPromesaPresupuesto= fechaPromesaPresupuesto;

            }
            if(numeroSerie!=string.Empty)
            {
                reparacion.NumeroSerie= numeroSerie;
            }
            if (descripcion != string.Empty)
            {

                reparacion.Descripcion= descripcion;
            }
            
            await _context.SaveChangesAsync();
            return reparacion;

        }


       
        public async Task<List<Reparacion>> HistoriaClinicaPorNumeroSerie(string numeroSerie)
        {
            List<Reparacion>HistoriaClinica= await _context.Reparaciones.Where(r=>r.NumeroSerie==numeroSerie && r.Estado=="Entregada" &&r.Reparada).ToListAsync();
            return HistoriaClinica;
        }

        public async Task<double> ObtenerMontoTotalReparaciones(string numeroSerie)
        {
            List<Reparacion>reparaciones = await HistoriaClinicaPorNumeroSerie(numeroSerie);
            double gastoTotal = reparaciones.Sum(r => r.CostoFinal);
            return gastoTotal;
        }
    }
}
