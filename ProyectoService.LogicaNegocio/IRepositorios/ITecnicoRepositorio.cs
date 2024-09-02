using ProyectoService.LogicaNegocio.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoService.LogicaNegocio.IRepositorios
{
    public interface ITecnicoRepositorio : ICrudRepositorio<Tecnico>
    {
        Task <Tecnico?> ObtenerTecnicoPorEmail(string email);
        Task<Tecnico?> ObtenerTecnicoPorId(int id);
        Task<bool> CambiarPassword(string email, byte[] passwordHash, byte[]passwordSalt);
        
       
    }
}
