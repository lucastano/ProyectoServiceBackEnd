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
    public class AgregarTecnico : IAgregarTecnico
    {
        private readonly ITecnicoRepositorio repo;

        public AgregarTecnico(ITecnicoRepositorio repo)
        {
            this.repo = repo;
        }

        public async Task Ejecutar(Tecnico entity)
        {
           await repo.Add(entity);
        }
    }
}
