using ProyectoService.LogicaNegocio.Modelo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoService.Aplicacion.ICasosUso
{
    public interface IObtenerTecnicoPorId
    {
        Task<Tecnico> Ejecutar(int id);
    }
}
