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
    public class AvisoNuevaReparacion : IAvisoNuevaReparacion
    {
        private readonly IEnviarEmail repoEmail;
        public AvisoNuevaReparacion(IEnviarEmail repoEmail)
        {
            this.repoEmail = repoEmail;
        }

        public async Task<byte[]> Ejecutar(Reparacion entity)
        {
            return await  repoEmail.EnviarEmailNuevaReparacion(entity);
        }
    }
}
