using ProyectoService.LogicaNegocio.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoService.Aplicacion.ICasosUso
{
    public interface IObtenerHistoriaClinica
    {

        Task<List<Reparacion>> Ejecutar(string numeroSerie);
    }
}
