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
    public class ObtenerClientePorId: IObtenerClientePorId
    {
        private readonly IClienteRepositorio repo;
        public ObtenerClientePorId(IClienteRepositorio repo)
        {
            this.repo = repo;
        }

        public async Task<Cliente> Ejecutar(int id)
        {
            return await repo.GetClienteById(id);
        }
    }
}
