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
    public class CambiarPasswordTecnico : ICambiarPasswordTecnico
    {
        private readonly ITecnicoRepositorio _repositorio;
        public CambiarPasswordTecnico(ITecnicoRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<bool> Ejecutar(string email, byte[] passwordHash, byte[]passwordSalt)
        {
           return await _repositorio.CambiarPassword(email, passwordHash, passwordSalt);
        }
    }
}
