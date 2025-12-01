namespace Firmeza.Application.Interfaces;

public interface IEmailService
{
    Task SendEmailWithPdf(string toEmail, string subject, string body, string pdfFullPath);
}