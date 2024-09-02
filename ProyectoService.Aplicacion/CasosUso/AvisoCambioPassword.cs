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
    public class AvisoCambioPassword : IAvisoCambioPassword
    {
        private readonly IEnviarEmail enviarEmail;
        public AvisoCambioPassword(IEnviarEmail enviarEmail)
        {
            this.enviarEmail = enviarEmail;
        }

        public async Task Ejecutar(Usuario usu, string password)
        {
            await enviarEmail.AvisoCambioPassword(usu, password);
        }
    }
}
