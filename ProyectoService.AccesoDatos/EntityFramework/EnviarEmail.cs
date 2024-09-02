using ProyectoService.LogicaNegocio.IRepositorios;
using ProyectoService.LogicaNegocio.Modelo;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;




namespace ProyectoService.AccesoDatos.EntityFramework
{
    public class EnviarEmail : IEnviarEmail
    {
       
        private readonly IConfiguration configuration;
        SmtpClient smtpClient;
        private Empresa empresa;
        
        
        
        public  EnviarEmail( IConfiguration configuration)
        {
            this.configuration = configuration;
            var configNombreEmpresa = configuration.GetSection("EmpresaSettings:NombreEmpresa").Value!;
            var configDireccionEmpresa = configuration.GetSection("EmpresaSettings:DireccionEmpresa").Value!;
            var configTelefonoEmpresa = configuration.GetSection("EmpresaSettings:TelefonoEmpresa").Value!;
            var configEmail = configuration.GetSection("EmpresaSettings:Email").Value!;
            var configPassword = configuration.GetSection("EmpresaSettings:EmailPassword").Value!;
            var configPoliticasEmpresa = configuration.GetSection("EmpresaSettings:PoliticasEmpresa").Value!;
            var emailServer = configuration.GetSection("EmpresaSettings:EmailServer").Value!;
            var apiKey= configuration.GetSection("EmpresaSettings:APIKEY").Value!;
            var secretKey = configuration.GetSection("EmpresaSettings:SECRETKEY").Value!;

            Empresa empresaConfig = new Empresa()
            {
                Nombre = configNombreEmpresa,
                Direccion = configDireccionEmpresa,
                Telefono = configTelefonoEmpresa,
                Email = configEmail,
                EmailPassword = configPassword,
                PoliticasEmpresa = configPoliticasEmpresa
            };
            this.empresa = empresaConfig;
           
            smtpClient = new SmtpClient("in.mailjet.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(apiKey, secretKey),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                

            };

            

        }

        public async Task AvisoCambioPassword(Usuario usu,string password)
        {
            string fromName = this.empresa.Nombre;
            string fromEmail = this.empresa.Email;
            string toName = usu.Nombre;
            string toEmail = usu.Email.Value;
            string subject = "RECUPERACION DE CONTRASEÑA: " + usu.Email.Value;
            
            string body = $@"
             <html>
             <body>
                 <p>Nueva contraseña generado para inicio de sesión.</p>
                 <p>Para mantener la proteccion de su usuario, por favor cambie la contraseña luego de iniciar.</p>
                 <p><strong>Contraseña:</strong> <strong>{password}</strong></p>
             </body>
             </html>";

            bool isHtml = true;

            MailMessage mailMessage = new MailMessage()
            {
                From = new MailAddress(fromEmail, fromName),
                Subject = subject,
                Body = body,
                IsBodyHtml = isHtml

            };
            mailMessage.To.Add(new MailAddress(toEmail, toName));
            await smtpClient.SendMailAsync(mailMessage);

        }

        public async Task<byte[]> EnviarEmailAvisoEntrega(Reparacion entity)
        {
            
            string reparada = "Reparada";
            if (!entity.Reparada)
            {
                reparada = "NO Reparada";
            }
            byte[] pdfContent = entity.GenerarPdfOrdenServicioEntregada(this.empresa);

            // Verifica si el PDF se generó correctamente
            if (pdfContent != null && pdfContent.Length > 0)
            {
                // Ver los datos de la empresa de donde obtenerlos, al igual que el email de envio
                string fromName = this.empresa.Nombre;
                string fromEmail = this.empresa.Email;
                string toName = entity.Cliente.Nombre;
                string toEmail = entity.Cliente.Email.Value;
                string subject = "REPARACION Entregada Nro: " + entity.Id;
                

                string body = $@"
             <html>
             <body>
                 <p>Se entrego el producto <strong>{entity.Producto.Marca}  {entity.Producto.Modelo} {entity.Producto.Version} </strong></p>
                 <p>Número de serie: <strong>{ entity.NumeroSerie} </strong></p>
                 <p>Número de orden:<strong> {entity.Id}</strong></p>
                 <p>Problema reportado: <strong> {entity.Descripcion}</strong></p>
                 <p>Presupuesto: <strong> {entity.DescripcionPresupuesto}</strong></p>
                 <p>Estado de la reparacion: <strong> {reparada}</strong></p>
                 <p>Fecha y hora de entrega: <strong> {entity.FechaEntrega}</strong></p>
                 <p>Importe abonado: <strong> {entity.CostoFinal}</strong></p>
                 <p>Muchas gracias por confiar en nuestro servicio</p>

             </body>
             </html>";
               bool isHtml = true;


                
                MailMessage mailMessage = new MailMessage()
                {
                    From = new MailAddress(fromEmail, fromName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = isHtml

                 };
                mailMessage.To.Add(new MailAddress(toEmail, toName));


           
            using (MemoryStream stream = new MemoryStream(pdfContent))
                {
                    mailMessage.Attachments.Add(new Attachment(stream, "orden_de_servicio_"+entity.Id+".pdf", "application/pdf"));

                    // Envía el correo electrónico
                    
                        try
                        {
                            await smtpClient.SendMailAsync(mailMessage);

                        }
                        catch (SmtpException ex)
                        {

                        }
                    
                }
            }
            return pdfContent;


        }

        public async Task EnviarEmailAvisoTerminada(Reparacion entity)
        {
           

            string fromName = this.empresa.Nombre;
            string fromEmail = this.empresa.Email;
            string toName = entity.Cliente.Nombre;
            string toEmail = entity.Cliente.Email.Value;
            string subject = "REPARACION TERMINADA ORDEN DE SERVICIO Nro: " + entity.Id;
            string body = $@"
             <html>
             <body>
                 <p>Reparacione Nro: <strong>{entity.Id}</strong></p>
                 <p>Producto: <strong>{entity.Producto.Marca+" "+entity.Producto.Modelo+" "+entity.Producto.Version}</strong></p>
                 <p>Numero de serie: <strong>{entity.NumeroSerie}</strong></p>
                 <p><strong>Se encuentra lista para ser retirada</strong></p>

             </body>
             </html>";
            bool isHtml = true;

          
            MailMessage mailMessage = new MailMessage()
            {
                From = new MailAddress(fromEmail, fromName),
                Subject = subject,
                Body = body,
                IsBodyHtml = isHtml

            };
            mailMessage.To.Add(new MailAddress(toEmail, toName));
            

                try
                {
                    await smtpClient.SendMailAsync(mailMessage);

                }
                catch (SmtpException ex)
                {

                }
        }

        public async Task<byte[]> EnviarEmailNuevaReparacion(Reparacion entity)
        {

            byte[] pdfContent = entity.GenerarPdfOrdenServicioEntrada(this.empresa);
            

            // Verifica si el PDF se generó correctamente
            if (pdfContent != null && pdfContent.Length > 0)
            {
                // Ver los datos de la empresa de donde obtenerlos, al igual que el email de envío
                string fromName = empresa.Nombre;
                string fromEmail = empresa.Email;
                string toName = entity.Cliente.Nombre;
                string toEmail = entity.Cliente.Email.Value;
                string subject = "ORDEN DE SERVICIO REPARACION Nro: " + entity.Id;

                string body = $@"
             <html>
             <body>
                 <p>Se dejó para service el aparato <strong>{entity.Producto.Marca + " " + entity.Producto.Modelo + " " + entity.Producto.Version}</strong></p>
                 <p>Numero de serie: <strong>{entity.NumeroSerie}</strong></p>
                 <p>Numero de orden: <strong>{entity.Id}</strong></p>
                 <p>Fecha aproximada del presupuesto: <strong>{entity.FechaPromesaPresupuesto.ToString("dd-MM-yyyy")}</strong></p>

             </body>
             </html>";
                bool isHtml = true;

                // Configura el mensaje de correo electrónico
                MailMessage mailMessage = new MailMessage()
                {
                    From = new MailAddress(fromEmail, fromName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = isHtml,
                    
                };
                mailMessage.To.Add(new MailAddress(toEmail, toName));

                
                mailMessage.ReplyToList.Add(new MailAddress("soporte@tudominio.com", "Soporte Técnico")); 
                mailMessage.Headers.Add("X-Mailer", "Microsoft Outlook 16.0"); 
                mailMessage.Headers.Add("X-Priority", "3");
               
                using (MemoryStream stream = new MemoryStream(pdfContent))
                {
                    mailMessage.Attachments.Add(new Attachment(stream, "orden_de_servicio_" + entity.Id + ".pdf", "application/pdf"));
                    try
                    {
                        await smtpClient.SendMailAsync(mailMessage);

                    }
                    catch (SmtpException ex)
                    {

                    }
   
                }

            }
            return pdfContent;

        }

        public async Task EnviarEmailNuevoPresupuesto(Reparacion entity)
        {
            byte[] pdfContent = entity.GenerarPdfOrdenServicioPresupuestada(this.empresa);
           
            // Verifica si el PDF se generó correctamente
            if (pdfContent != null && pdfContent.Length > 0)
            {
                // Ver los datos de la empresa de donde obtenerlos, al igual que el email de envio
                string fromName = this.empresa.Nombre;
                string fromEmail = this.empresa.Email;
                string toName = entity.Cliente.Nombre;
                string toEmail = entity.Cliente.Email.Value;
                string subject = "PRESUPUESTO ORDEN DE SERVICIO REPARACION Nro: " + entity.Id;
                string body = $@"
             <html>
             <body>
                 <p>Presupuesto para la orden Nro: {entity.Id}</p>
                 <p>Producto: <strong>{entity.Producto.Marca + " " + entity.Producto.Modelo + " " + entity.Producto.Version}</strong></p>
                 <p>Numero de serie: <strong>{entity.NumeroSerie}</strong></p>
                 <p>Descripcion del problema: <strong>{entity.Descripcion}</strong></p>
                 <p>Descripcion de presupuesto: <strong>{entity.DescripcionPresupuesto}</strong></p>
                 <p>Costo de reparacion: <strong>{entity.CostoFinal}</strong></p>
                 <p>Fecha aproximada de entrega: <strong>{entity.FechaPromesaEntrega}</strong></p>
                 <p><strong>Para aceptar el presupuesto comuniquese con la empresa. Gracias</strong></p>


             </body>
             </html>";
                bool isHtml = true;

                // Configura el mensaje de correo electrónico
                MailMessage mailMessage = new MailMessage()
                {
                    From = new MailAddress(fromEmail, fromName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = isHtml

                };
                
                mailMessage.To.Add(new MailAddress(toEmail, toName));
               

                // Adjunta el PDF al correo electrónico
                using (MemoryStream stream = new MemoryStream(pdfContent))
                {
                    mailMessage.Attachments.Add(new Attachment(stream, "orden_de_servicio_" + entity.Id + ".pdf", "application/pdf"));

                    // Envía el correo electrónico
                    try
                    {
                        await smtpClient.SendMailAsync(mailMessage);


                    }
                    catch (SmtpException ex)
                    {

                    }
                }
            }
            
        }
        




    }
}
