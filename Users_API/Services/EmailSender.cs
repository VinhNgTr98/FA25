using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;

namespace UserManagement_API.Services
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly SmtpOptions _opt;
        public SmtpEmailSender(IOptions<SmtpOptions> opt) => _opt = opt.Value;

        public async Task SendAsync(string toEmail, string subject, string htmlBody, CancellationToken ct = default)
        {
            using var client = new SmtpClient(_opt.Host, _opt.Port)
            {
                EnableSsl = _opt.EnableSsl,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_opt.Username, _opt.Password)
            };

            using var msg = new MailMessage
            {
                From = new MailAddress(_opt.From, _opt.FromDisplayName),
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true
            };
            msg.To.Add(new MailAddress(toEmail));

            await Task.Run(() => client.Send(msg), ct);
        }
    }

    public class SmtpOptions
    {
        public string Host { get; set; } = default!;
        public int Port { get; set; } = 587;
        public bool EnableSsl { get; set; } = true;
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string From { get; set; } = default!;
        public string FromDisplayName { get; set; } = "Support";
    }
}