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
    public class AgregarABaseFallas : IAgregarABaseFallas
    {
        private readonly IBaseFallaRepositorio repo;
        public AgregarABaseFallas(IBaseFallaRepositorio repo)
        {
            this.repo = repo;
        }

        public async Task Ejecutar(BaseFalla baseFalla)
        {
            await repo.Add(baseFalla);
        }
    }
}
