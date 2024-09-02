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
    public class GenerarOrdenDeServicio : IGenerarOrdenDeServicio
    {
        private readonly IReparacionRepositorio repo;
        public GenerarOrdenDeServicio(IReparacionRepositorio repo)
        {
            this.repo = repo;
        }

        public byte[] Ejecutar(Reparacion rep, Empresa emp)
        {
           return repo.GenerarOrdenDeServicio(rep, emp);
        }
    }
}
