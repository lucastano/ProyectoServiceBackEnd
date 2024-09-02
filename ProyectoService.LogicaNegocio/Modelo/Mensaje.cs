using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoService.LogicaNegocio.Modelo
{
    [Table("mensajes")]
    public class Mensaje
    {

        public int Id { get; set; }
        public int ReparacionId {  get; set; }

        public Usuario Emisor {  get; set; }

        public int EmisorId { get; set; }
        public Usuario Destinatario { get; set; }
        public int DestinatarioId {  get; set; }

        public DateTime FechaHoraEnvio { get; set; }
        public Reparacion Reparacion { get; set; }
        public string Texto {  get; set; }
    }
}
