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

        
        Task<Reparacion>ObtenerReparacionPorId(int id);
        Task<List<Reparacion>> ObtenerReparacionesPorCliente(string Ci);
        Task<List<Reparacion>> ObtenerReparacionesPorTecnico(string EmailTecnico);
        byte[] GenerarOrdenDeServicio(Reparacion rep,Empresa emp);
        Task<Reparacion> AddAlternativo(Reparacion entity);
        Task<Reparacion> ModificarPresupuestoReparacion(int id, double costo, string descripcion);
        Task<Reparacion> ModificarDatosReparacion(int id, DateTime fechaPromesaPresupuesto,string numeroSerie,string descripcion);
        Task<List<Reparacion>> HistoriaClinicaPorNumeroSerie(string numeroSerie);
        Task<double> ObtenerMontoTotalReparaciones(string numeroSerie);


    }
}
