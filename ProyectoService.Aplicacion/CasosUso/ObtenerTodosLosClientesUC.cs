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
    public class ObtenerTodosLosClientesUC : IObtenerTodosLosClientesUC
    {
        private readonly IClienteRepositorio repo;
        public ObtenerTodosLosClientesUC(IClienteRepositorio repo)
        {
            this.repo = repo;
        }

        public async Task<List<Cliente>> Ejecutar()
        {
            return await repo.getAll();
        }
    }
}
