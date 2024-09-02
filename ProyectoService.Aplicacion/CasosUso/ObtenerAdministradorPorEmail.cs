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
    public class ObtenerAdministradorPorEmail : IObtenerAdministradorPorEmail
    {
        private readonly IAdministradorRepositorio repo;
        public ObtenerAdministradorPorEmail(IAdministradorRepositorio repo)
        {
            this.repo = repo;
        }

        public async Task<Administrador> Ejecutar(string email)
        {
            return await repo.ObtenerAdministradorPorEmail(email);
        }
    }
}
