using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;
using MailKit.Security;



namespace SmartPowerElectricAPI.Service
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;


        public EmailService(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        public async Task SendMailAsync(string MailTo, string Topic, string Body, List<string> Attachments = null, List<string> ccEmails=null)
        {
            string RouteFirm= System.IO.Path.Combine(_env.ContentRootPath, "Assets", "Img", "Firma.jpg");

            var MailSend = new MimeMessage();
            MailSend.From.Add(new MailboxAddress(
                _configuration["SmtpSettings:SenderName"],
                _configuration["SmtpSettings:SenderEmail"]
            ));
            MailSend.To.Add(MailboxAddress.Parse(MailTo));

          

            MailSend.Subject = Topic;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = Body,  // Correo en HTML
                TextBody = "Este correo no soporta HTML"  // Texto sin formato
            };

            if (Attachments!=null)
            {
                // Agregar adjuntos
                foreach (var archivo in Attachments)
                {
                    bodyBuilder.Attachments.Add(archivo);
                }
            }

            if (ccEmails!=null)
            {
                // Agregar personas en copia (CC)
                foreach (var email in ccEmails)
                {
                    MailSend.Cc.Add(MailboxAddress.Parse(email));
                }

            }
           

            // Crear la imagen de firma como MimePart
            var firma = new MimePart("image", "png")
            {
                Content = new MimeContent(File.OpenRead(RouteFirm)),
                ContentDisposition = new ContentDisposition(ContentDisposition.Inline), // Corregido aquí
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = "firma.png",
                ContentId = "firmaImagen"
            };

            // Agregar la firma como recurso vinculado
            bodyBuilder.LinkedResources.Add(firma);

            // Insertar la imagen de firma en el cuerpo del correo
            bodyBuilder.HtmlBody += "<br><img src=\"cid:firmaImagen\" alt=\"Firma\" />";

            MailSend.Body = bodyBuilder.ToMessageBody();

            using var cliente = new SmtpClient();
            await cliente.ConnectAsync(
                _configuration["SmtpSettings:Server"],
                int.Parse(_configuration["SmtpSettings:Port"]),
                SecureSocketOptions.StartTls
            );

            await cliente.AuthenticateAsync(
                _configuration["SmtpSettings:Username"],
                _configuration["SmtpSettings:Password"]
            );

            await cliente.SendAsync(MailSend);
            await cliente.DisconnectAsync(true);
        }
    }
}