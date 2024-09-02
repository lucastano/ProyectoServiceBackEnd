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
    public class AgregarClienteUC : IAgregarClienteUC
    {
        private readonly IClienteRepositorio repo;

        public AgregarClienteUC(IClienteRepositorio repo)
        {
            this.repo = repo;
        }

        public async Task Ejecutar(Cliente cli)
        {
            
              await repo.Add(cli);
        }
    }
}
