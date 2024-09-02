using ProyectoService.Aplicacion.ICasosUso;
using ProyectoService.LogicaNegocio.IRepositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoService.Aplicacion.CasosUso
{
    public class ObtenerMontoTotalHistoriaClinica : IObtenerMontoTotalHistoriaClinica
    {
        private readonly IReparacionRepositorio repo;
        public ObtenerMontoTotalHistoriaClinica(IReparacionRepositorio repo)
        {
            this.repo = repo;
        }

        public async Task<double> Ejecutar(string numeroSerie)
        {
            return await repo.ObtenerMontoTotalReparaciones(numeroSerie);
        }
    }
}
