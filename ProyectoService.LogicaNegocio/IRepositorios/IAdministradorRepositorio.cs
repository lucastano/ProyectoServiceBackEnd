using ProyectoService.LogicaNegocio.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoService.LogicaNegocio.IRepositorios
{
    public interface IAdministradorRepositorio:ICrudRepositorio<Administrador>
    {
        Task<Administrador> ObtenerAdministradorPorEmail(string email);
        Task<bool> CambiarPassword(string email ,byte[] passwordHash, byte[] passwordSalt);
    }
}
