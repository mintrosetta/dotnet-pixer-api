using PixerAPI.Services.Interfaces;
using System.Net;
using System.Net.Mail;

namespace PixerAPI.Services
{
    public class EmailServices : IEmailService
    {
        private readonly IConfiguration configuration;

        public EmailServices(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task SendAsync(string to, string subject, string body)
        {
            string host = this.configuration.GetSection("Mail:Smtp:Host").Value!;
            string port = this.configuration.GetSection("Mail:Smtp:Port").Value!;
            string user = this.configuration.GetSection("Mail:Smtp:User").Value!;
            string password = this.configuration.GetSection("Mail:Smtp:Password").Value!;
            string sender = this.configuration.GetSection("Mail:Smtp:Sender").Value!;

            MailMessage message = new MailMessage(sender, to, subject, body);
            message.IsBodyHtml = false;

            using SmtpClient smtpClient = new SmtpClient(host, Convert.ToInt32(port));
            smtpClient.Credentials = new NetworkCredential(user, password);
            
            await smtpClient.SendMailAsync(message);
        }
    }
}
