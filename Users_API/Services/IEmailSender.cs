﻿namespace UserManagement_API.Services
{
    public interface IEmailSender
    {
        Task SendAsync(string toEmail, string subject, string htmlBody, CancellationToken ct = default);
    }
}
