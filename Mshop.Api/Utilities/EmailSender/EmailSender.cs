using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace Mshop.Api.Utilities.EmailSender
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var fromEmail = _configuration["EmailSettings:FromEmail"];
            var password = _configuration["EmailSettings:Password"];

            using SmtpClient smtpClient = new("smtp.gmail.com", 587)
            {
                UseDefaultCredentials=false,
                Credentials = new NetworkCredential(fromEmail, password),
                EnableSsl = true
            };

            MailMessage message = new(fromEmail!, email, subject, htmlMessage)
            {
                IsBodyHtml = true
            };

            await smtpClient.SendMailAsync(message);
        }
    }
}
