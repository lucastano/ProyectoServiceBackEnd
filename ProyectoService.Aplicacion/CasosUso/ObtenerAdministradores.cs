
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
    public class ObtenerAdministradores : IObtenerAdministradores
    {
        private readonly IAdministradorRepositorio repo;

        public ObtenerAdministradores(IAdministradorRepositorio repo)
        {
            this.repo = repo;
        }

        public async Task<List<Administrador>> Ejecutar()
        {
            return await repo.getAll();
        }
    }
}
