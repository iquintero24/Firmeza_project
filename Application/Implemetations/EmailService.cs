using System.Net;
using System.Net.Mail;
using Firmeza.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Firmeza.Application.Implemetations;

public class EmailService: IEmailService
{
    
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    
    public async Task SendEmailWithPdf(string toEmail, string subject, string body, string pdfFullPath)
    {
        using var smtp = new SmtpClient
        {
            Host = _configuration["Email:Host"],
            Port = int.Parse(_configuration["Email:Port"]),
            Credentials = new NetworkCredential(
                _configuration["Email:User"],
                _configuration["Email:Password"]
            ),
            EnableSsl = true
        };

        using var message = new MailMessage
        {
            From = new MailAddress(_configuration["Email:User"]),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };
        
        message.To.Add(toEmail);
        
        if(File.Exists(pdfFullPath))
            message.Attachments.Add(new Attachment(pdfFullPath));
        
        await smtp.SendMailAsync(message);

    }
}