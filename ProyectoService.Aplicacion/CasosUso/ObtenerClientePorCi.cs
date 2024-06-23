
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
    public class ObtenerClientePorCi : IObtenerClientePorCI
    {
        private readonly IClienteRepositorio repo;
        public ObtenerClientePorCi(IClienteRepositorio repo)
        {
            this.repo = repo;
        }

        public async Task<Cliente> Ejecutar(string ci)
        {
            if (ci == null || ci == "") throw new Exception("Cedula no puede ser vacia");
            return await repo.GetClienteByCi(ci);
        }
    }
}
