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
    public class AgregarAdministrador : IAgregarAdministrador
    {
        private readonly IAdministradorRepositorio repo;
        public AgregarAdministrador(IAdministradorRepositorio repo)
        {
            this.repo = repo;
        }

        public async Task Ejecutar(Administrador entity)
        {
            await repo.Add(entity);
        }
    }
}
