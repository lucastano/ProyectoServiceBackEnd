using ProyectoService.LogicaNegocio.IRepositorios;
using ProyectoService.LogicaNegocio.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoService.AccesoDatos.EntityFramework
{
    public class EnviarEmail : IEnviarEmail
    {
        private readonly MailService _mailService;

        public EnviarEmail()
        {
          
            //ver como obtener estos datos de algun archivo de texto
            _mailService = new MailService("smtp.office365.com", 587, "pruebaemial.net@outlook.es", "Lu3472759");
        }

        public Task EnviarEmailAvisoEntrega(Reparacion entity)
        {
            throw new NotImplementedException();
        }

        public Task EnviarEmailAvisoTerminada(Reparacion entity)
        {
            throw new NotImplementedException();
        }

        public async Task EnviarEmailNuevaReparacion(Reparacion entity)
        {
            //ver los datos de la empresa de donde obtenerlos, al igual que el email de envio
            //esta version es la primera sin archivo pdf
            //await _mailService.SendEmailAsync(
            //    fromName: "CompanyName",
            //    fromEmail: "pruebaemial.net@outlook.es",
            //    toName: entity.Cliente.Nombre,
            //    toEmail: entity.Cliente.Email.Value,
            //    subject: "ORDEN DE SERVICIO REPARACION Nro: " + entity.Id,
            //    body: "Se dejo para service el aparato" + entity.Producto + " Numero de serie: " + entity.NumeroSerie + " Su numero de orden es: " + entity.Id,
            //    isHtml: true


            //    );
            // Genera el PDF
            byte[] pdfContent = entity.GenerarPdfOrdenServicioEntrada();

            // Verifica si el PDF se generó correctamente
            if (pdfContent != null && pdfContent.Length > 0)
            {
                // Ver los datos de la empresa de donde obtenerlos, al igual que el email de envio
                string fromName = "CompanyName";
                string fromEmail = "pruebaemial.net@outlook.es";
                string toName = entity.Cliente.Nombre;
                string toEmail = entity.Cliente.Email.Value;
                string subject = "ORDEN DE SERVICIO REPARACION Nro: " + entity.Id;
                string body = "Se dejó para service el aparato " + entity.Producto + ". Número de serie: " + entity.NumeroSerie + ". Su número de orden es: " + entity.Id;
                bool isHtml = true;

                // Configura el mensaje de correo electrónico
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(fromEmail, fromName);
                mailMessage.To.Add(new MailAddress(toEmail, toName));
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = isHtml;

                // Adjunta el PDF al correo electrónico
                using (MemoryStream stream = new MemoryStream(pdfContent))
                {
                    mailMessage.Attachments.Add(new Attachment(stream, "orden_de_servicio.pdf", "application/pdf"));

                    // Envía el correo electrónico
                    using (SmtpClient smtpClient = new SmtpClient("smtp.outlook.com", 587))
                    {
                        smtpClient.Credentials = new NetworkCredential(fromEmail, "Lu3472759");
                        smtpClient.EnableSsl = true;
                        await smtpClient.SendMailAsync(mailMessage);
                    }
                }
            }
            else
            {
                // Si el PDF no se generó correctamente, muestra un mensaje de error o maneja la situación según sea necesario
                Console.WriteLine("No se pudo enviar el correo electrónico porque no se generó el PDF correctamente.");
            }

        }

        public async Task EnviarEmailNuevoPresupuesto(Reparacion entity)
        {
            await _mailService.SendEmailAsync(
                fromName: "CompanyName",
                fromEmail: "pruebaemial.net@outlook.es",
                toName: entity.Cliente.Nombre,
                toEmail: entity.Cliente.Email.Value,
                subject: "NUEVO PRESUPUESTO REPARACION Nro: " + entity.Id,
                body: "Nuevo presupuesto para" + entity.Producto + " Numero de serie: " + entity.NumeroSerie + " numero de orden: " + entity.Id+" : "+entity.DescripcionPresupuesto+ "con un costo de: "+entity.CostoFinal,
                isHtml: true
                ) ;
        }

        
    }
}
