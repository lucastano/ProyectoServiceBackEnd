
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace ProyectoService.LogicaNegocio.Modelo
{
    public class Reparacion
    {

        public int Id { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;

        public Tecnico Tecnico { get; set; }
        public Cliente Cliente { get; set; }
        public string Producto { get; set; }//esto va a pasar a ser un objeto
        public string NumeroSerie { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaPromesaPresupuesto { get; set; }
        public string Estado { get; set; }//ver de hacer un enum
        public string DescripcionPresupuesto { get; set; }
        public string RazonNoAceptada {  get; set; }
        public DateTime FechaPresupuesto { get; set; }
        public DateTime FechaPromesaEntrega { get; set; }
        public double ManoDeObra { get; set; }
        public double CostoFinal { get; set; }
        public DateTime FechaEntrega { get; set; }
        public bool Reparada {  get; set; }
        //al Terminar la reparacion, deberia agregar una descripcion de reparacion, de no repararse por no aceptar presupuesto, tambien deberia agregarla, el tenico puede indicar 
        //porque no se reparo 

        //TODO: DEBERIA SETEAR LOS DATOS QUE NO VOY A UTILIZAR CUANDO CREO EL OBJETO(COSTO,MANOOBRA,DESCRIPCIONPRESUPUESTO,REPUESTOS)????
        public Reparacion()
        {

            this.Estado = "EnTaller";
            this.DescripcionPresupuesto = string.Empty;
            this.ManoDeObra = 0;
            this.CostoFinal = 0;
            this.RazonNoAceptada = string.Empty;
            
        }
        

        public void Presupuestar(double costo,string descripcion,DateTime fechaPromesaEntrega)
        {
            this.ManoDeObra = costo;
            this.CostoFinal = ManoDeObra;
            this.Estado = "Presupuestada";
            this.DescripcionPresupuesto=descripcion;
            this.FechaPresupuesto=DateTime.Now;
            this.FechaPromesaEntrega = fechaPromesaEntrega;
            //TODO: para calcular el costo final, este metodo debe recibir la lista de repuestos a utilizar, se recorre esa lista y por cada repuesto se toma el costo
            //ese costo se suma en una variable que al finalizar vamos a utilizar para sumarle  a la mano de obra y ahi obtenemos el costo final
            //TODO: ACA TENEMOS QUE MANEJAR EL ENVIO DEL EMAIL
            
        }

        public void AceptarPresupuesto()
        {
            this.Estado = "PresupuestoAceptado";
        }
        public void NoAceptarPresupuesto(double costo,string razon)
        {
            this.Estado = "PresupuestoNoAceptado";
            this.RazonNoAceptada = razon;
            this.CostoFinal= costo;
        }
        public void Terminar(bool reparada)
        {
            if (!reparada)
            {
                //si no se realizo la reparacion, el costo final es 0, se mantiene la mano de obra para tener esa info
                this.CostoFinal = 0;
            }
            this.Estado = "Terminada";
            this.Reparada = reparada;

        }

        public void Entregar()
        {   
            this.FechaEntrega=DateTime.Now;  
            this.Estado = "Entregada";     
        }

        public byte[] GenerarPdfOrdenServicioEntrada()
        {
            // Crea un nuevo documento PDF
            PdfDocument document = new PdfDocument();
            document.Info.Title = "PDF Orden de servicio";




            // Crea una nueva página
            PdfPage page = document.AddPage();

            // Obtiene el objeto XGraphics para dibujar en la página
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Define la fuente
            XFont font = new XFont("Verdana", 20);

            // Dibuja texto en la página
            gfx.DrawString("Información de la Reparacion:", font, XBrushes.Black,
                new XRect(0, 0, page.Width, page.Height),
                XStringFormats.TopCenter);

            font = new XFont("Verdana", 12, XFontStyle.Regular);
            gfx.DrawString($"Numero de orden: {Id}", font, XBrushes.Black,
                new XRect(40, 60, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString($"Nombre de cliente: {Cliente.Nombre}", font, XBrushes.Black,
                new XRect(40, 90, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString($"Descripcion de problema: {Descripcion}", font, XBrushes.Black,
                new XRect(40, 120, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString($"Producto: {Producto}", font, XBrushes.Black,
                new XRect(40, 150, page.Width, page.Height),
                XStringFormats.TopLeft);

            // Guarda el documento en un MemoryStream
            using (MemoryStream stream = new MemoryStream())
            {
                document.Save(stream, false);
                return stream.ToArray();
            }

        }




 




    }
}
