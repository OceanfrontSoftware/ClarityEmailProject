using EmailService.DataObjects;

namespace EmailService.Services
{
    public interface IEmailService
    {
        Task<int> SendEmailAsync(Message message);
    }
} 