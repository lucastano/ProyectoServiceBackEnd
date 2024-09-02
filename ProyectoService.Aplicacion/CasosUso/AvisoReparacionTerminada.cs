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
    public class AvisoReparacionTerminada : IAvisoReparacionTerminada
    {
        private readonly IEnviarEmail repo;

        public AvisoReparacionTerminada(IEnviarEmail repo)
        {
            this.repo = repo;
        }

        public async Task Ejecutar(Reparacion entity)
        {
            await repo.EnviarEmailAvisoTerminada(entity);
        }
    }
}
