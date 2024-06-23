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
    public class ObtenerTecnicoPorId : IObtenerTecnicoPorId
    {
        private readonly ITecnicoRepositorio repo;
        public ObtenerTecnicoPorId(ITecnicoRepositorio repo)
        {
            this.repo = repo;
        }

        public async Task<Tecnico> Ejecutar(int id)
        {


            if (id == 0) throw new Exception("Debe ingresar tecnico");
            return await repo.ObtenerTecnicoPorId(id);
        }
    }
}
