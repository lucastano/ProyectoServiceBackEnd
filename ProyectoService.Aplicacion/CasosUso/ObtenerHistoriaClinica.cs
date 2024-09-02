using ProyectoService.Aplicacion.ICasosUso;
using ProyectoService.LogicaNegocio.IRepositorios;
using ProyectoService.LogicaNegocio.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoService.Aplicacion.CasosUso
{
    public class ObtenerHistoriaClinica : IObtenerHistoriaClinica
    {

        private readonly IReparacionRepositorio repo;

        public ObtenerHistoriaClinica(IReparacionRepositorio repo)
        {
            this.repo = repo;
        }

        public async Task<List<Reparacion>> Ejecutar(string numeroSerie)
        {
            return await repo.HistoriaClinicaPorNumeroSerie(numeroSerie);
        }
    }
}
