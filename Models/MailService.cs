using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var smtpSettings = _config.GetSection("SmtpSettings");

        string? senderEmail = smtpSettings["SenderEmail"];
        string? senderName = smtpSettings["SenderName"];
        string? username = smtpSettings["Username"];
        string? password = smtpSettings["Password"];
        string? server = smtpSettings["Server"];
        string? portString = smtpSettings["Port"];

        // Null kontrolü
        if (string.IsNullOrEmpty(senderEmail))
            throw new ArgumentNullException(nameof(senderEmail), "Sender email cannot be null or empty.");
        if (string.IsNullOrEmpty(senderName))
            throw new ArgumentNullException(nameof(senderName), "Sender name cannot be null or empty.");
        if (string.IsNullOrEmpty(username))
            throw new ArgumentNullException(nameof(username), "Username cannot be null or empty.");
        if (string.IsNullOrEmpty(password))
            throw new ArgumentNullException(nameof(password), "Password cannot be null or empty.");
        if (string.IsNullOrEmpty(server))
            throw new ArgumentNullException(nameof(server), "Server cannot be null or empty.");
        if (!int.TryParse(portString, out int port))
            throw new ArgumentException("Invalid port number.", nameof(portString));

        var smtpClient = new SmtpClient(server)
        {
            Port = port,
            Credentials = new NetworkCredential(username, password),
            EnableSsl = true,
            

        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(senderEmail, senderName),
            Subject = subject,
            Body = body,
            IsBodyHtml = true,
            
        };

        // Alıcı e-posta adresi null kontrolü
        if (string.IsNullOrEmpty(toEmail))
            throw new ArgumentNullException(nameof(toEmail), "Recipient email cannot be null or empty.");

        mailMessage.To.Add(toEmail);

        await smtpClient.SendMailAsync(mailMessage);
    }
}
