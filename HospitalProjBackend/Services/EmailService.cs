using MailKit.Net.Smtp;
using MimeKit;
using System.Net.Mail;

namespace HospitalProjBackend.Services
{
    public interface IEmailService { Task SendAsync(string to, string subject, string body); }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _cfg;
        public EmailService(IConfiguration cfg) { _cfg = cfg; }
        public async Task SendAsync(string to, string subject, string body)
        {
            var msg = new MimeMessage();
            msg.From.Add(MailboxAddress.Parse(_cfg["Email:Username"] ?? "no-reply@example.com"));
            msg.To.Add(MailboxAddress.Parse(to));
            msg.Subject = subject;
            msg.Body = new TextPart("plain") { Text = body };

            using var client = new MailKit.Net.Smtp.SmtpClient();
            // using var client = new SmtpClient();
            var host = _cfg["Email:SmtpServer"] ?? throw new Exception("Missing SMTP server");
            var port = int.Parse(_cfg["Email:SmtpPort"] ?? "587");
            var user = _cfg["Email:Username"];
            var pass = _cfg["Email:Password"];
            await client.ConnectAsync(host, port, MailKit.Security.SecureSocketOptions.StartTls);
            if (!string.IsNullOrEmpty(user))
                await client.AuthenticateAsync(user, pass);
            await client.SendAsync(msg);
            await client.DisconnectAsync(true);
        }
    }
}