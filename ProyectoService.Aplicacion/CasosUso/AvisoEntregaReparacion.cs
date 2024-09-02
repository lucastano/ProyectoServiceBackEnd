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
    public class AvisoEntregaReparacion : IAvisoEntregaReparacion
    {
        private readonly IEnviarEmail repo;
        public AvisoEntregaReparacion(IEnviarEmail repo)
        {
            this.repo = repo;
        }

        public async Task<byte[]> Ejecutar(Reparacion entity)
        {
            return await repo.EnviarEmailAvisoEntrega(entity);
        }
    }
}
