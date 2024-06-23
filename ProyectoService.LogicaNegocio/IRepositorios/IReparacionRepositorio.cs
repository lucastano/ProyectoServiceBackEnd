using ProyectoService.LogicaNegocio.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoService.LogicaNegocio.IRepositorios
{
    public interface IReparacionRepositorio:ICrudRepositorio<Reparacion>
    {
        Task<Reparacion> Presupuestar(int id,double ManoObra,string Descripcion,DateTime fechaPromesaEntrega);
        Task<Reparacion> Entregar(int id);
        Task AceptarPresupuesto(int id);
        Task NoAceptarPresupuesto(int id,double costo,string razon);
        Task<Reparacion> Terminar(int id, bool reparada);

        //estas reparaciones son todas las reparaciones sin ver su estado
        Task<Reparacion>ObtenerReparacionPorId(int id);
        Task<List<Reparacion>> ObtenerReparacionesPorCliente(string Ci);
        Task<List<Reparacion>> ObtenerReparacionesPorTecnico(string EmailTecnico);
        //estas reparaciones son presupuestadas
        Task<List<Reparacion>> ObtenerReparacionesPresupuestadas();
        Task<List<Reparacion>> ObtenerReparacionesPresupuestadasPorCliente(string ci);
        Task<List<Reparacion>> ObtenerReparacionesPresupuestadasPorTecnico(string EmailTecnico);
        //estas reparaciones en taller sin presupuestar 
        Task<List<Reparacion>> ObtenerReparacionesEnTaller();
        Task<List<Reparacion>> ObtenerReparacionesEnTallerPorCliente(string ci);
        Task<List<Reparacion>> ObtenerReparacionesEnTallerPorTecnico(string EmailTecnico);

        //estas reparaciones estan terminadas 
        Task<List<Reparacion>> ObtenerReparacionesTerminadas();//todas las entregadas
        Task<List<Reparacion>> ObtenerReparacionesTerminadasPorCliente(string ci);//entregadas por cliente
        Task<List<Reparacion>> ObtenerReparacionesTerminadasPorTecnico(string EmailTecnico);//entregadas por tecnico


        //estas reparaciones estan entregadas
        //los filtrados por reparadas o no reparadas las hacemos en el front para no hacer tantos llamados a la api
        Task<List<Reparacion>> ObtenerReparacionesEntregadas();//todas las entregadas
        Task<List<Reparacion>> ObtenerReparacionesEntregadasPorCliente(string ci);//entregadas por cliente
        Task<List<Reparacion>> ObtenerReparacionesEntregadasPorTecnico(string EmailTecnico);//entregadas por tecnico
        //TODO:la hice para probar 
        Task<Reparacion> AddAlternativo(Reparacion entity);



    }
}
