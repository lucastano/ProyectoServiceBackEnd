using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoService.LogicaNegocio.Modelo
{
    [Table("baseFallas")]
    public class BaseFalla
    {
        public int Id { get; set; }
        public Producto Producto { get; set; }
        public string Falla { get; set; }
        public string Solucion { get; set; }
    }
}
