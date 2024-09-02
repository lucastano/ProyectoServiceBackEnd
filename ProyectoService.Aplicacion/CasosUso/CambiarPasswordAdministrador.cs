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
    public class CambiarPasswordAdministrador : ICambiarPasswordAdministrador
    {
        private readonly IAdministradorRepositorio repo;
        public CambiarPasswordAdministrador(IAdministradorRepositorio repo)
        {
            this.repo = repo;
        }


        public async Task<bool> Ejecutar(string email, byte[] passwordHash, byte[] passwordSalt)
        {
            return await repo.CambiarPassword(email,passwordHash,passwordSalt);

        }
    }
}
