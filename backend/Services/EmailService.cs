using MailKit.Net.Smtp;
using MimeKit;
using backend.Services; 

namespace backend.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task EnviarCorreoConfirmacionAsync(string destino, string nombreCliente, string fecha, string hora)
        {
            var emailSettings = _config.GetSection("EmailSettings");
            var mensaje = new MimeMessage();

            mensaje.From.Add(new MailboxAddress(
                emailSettings["SenderName"] ?? "Despacho Jurídico", 
                emailSettings["SenderEmail"]!
            ));
            
            mensaje.To.Add(new MailboxAddress(nombreCliente, destino));
            mensaje.Subject = "Confirmación de Asesoría Jurídica";

            var builder = new BodyBuilder();
            builder.HtmlBody = $@"
                <div style='font-family: sans-serif; max-width: 600px; border: 1px solid #eee; padding: 20px;'>
                    <h2 style='color: #8B0000;'>Hola, {nombreCliente}</h2>
                    <p>Tu pago ha sido procesado exitosamente y tu asesoría ha quedado agendada.</p>
                    <hr>
                    <p><strong>Fecha:</strong> {fecha}</p>
                    <p><strong>Hora:</strong> {hora} hrs</p>
                    <p><strong>Ubicación:</strong> Videollamada (El link se enviará 15 min antes)</p>
                    <hr>
                    <p style='font-size: 12px; color: #666;'>Si necesitas reprogramar, por favor contáctanos con 24 horas de anticipación.</p>
                </div>";

            mensaje.Body = builder.ToMessageBody();

            using var clienteSmtp = new SmtpClient();
            var puerto = int.Parse(emailSettings["SmtpPort"] ?? "587");

            await clienteSmtp.ConnectAsync(emailSettings["SmtpServer"]!, puerto, MailKit.Security.SecureSocketOptions.StartTls);
            await clienteSmtp.AuthenticateAsync(emailSettings["SmtpUser"]!, emailSettings["SmtpPass"]!);
            await clienteSmtp.SendAsync(mensaje);
            await clienteSmtp.DisconnectAsync(true);
        }

        // --- NUEVO MÉTODO PARA BIENVENIDA ---
        public async Task EnviarCorreoBienvenidaAsync(string destino, string nombreUsuario)
        {
            var emailSettings = _config.GetSection("EmailSettings");
            var mensaje = new MimeMessage();

            mensaje.From.Add(new MailboxAddress(
                emailSettings["SenderName"] ?? "Despacho Jurídico Brizuela", 
                emailSettings["SenderEmail"]!
            ));
            
            mensaje.To.Add(new MailboxAddress(nombreUsuario, destino));
            mensaje.Subject = "¡Bienvenido a Despacho Jurídico Brizuela!";

            var builder = new BodyBuilder();
            builder.HtmlBody = $@"
                <div style='font-family: sans-serif; max-width: 600px; border: 1px solid #eee; padding: 20px;'>
                    <h2 style='color: #8B0000;'>¡Hola, {nombreUsuario}!</h2>
                    <p>Nos alegra darte la bienvenida a nuestra plataforma de gestión legal.</p>
                    <p>Tu cuenta ha sido creada exitosamente. A partir de ahora, podrás agendar asesorías y dar seguimiento a tus expedientes directamente desde tu portal.</p>
                    <hr>
                    <p style='font-size: 12px; color: #666;'>Si no creaste esta cuenta, por favor ignora este correo.</p>
                </div>";

            mensaje.Body = builder.ToMessageBody();

            using var clienteSmtp = new SmtpClient();
            var puerto = int.Parse(emailSettings["SmtpPort"] ?? "587");

            await clienteSmtp.ConnectAsync(emailSettings["SmtpServer"]!, puerto, MailKit.Security.SecureSocketOptions.StartTls);
            await clienteSmtp.AuthenticateAsync(emailSettings["SmtpUser"]!, emailSettings["SmtpPass"]!);
            await clienteSmtp.SendAsync(mensaje);
            await clienteSmtp.DisconnectAsync(true);
        }
    }
}