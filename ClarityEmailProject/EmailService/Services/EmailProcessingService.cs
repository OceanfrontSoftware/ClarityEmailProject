using EmailService.DataObjects;
using Microsoft.EntityFrameworkCore;

namespace EmailService.Services
{
    public class EmailProcessingService : IEmailService
    {
        private readonly EmailDbContext _dbContext;

        public EmailProcessingService(EmailDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> SendEmailAsync(Message message)
        { 
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            _dbContext.Messages.Add(message);
            await _dbContext.SaveChangesAsync();

            return message.Id;
        }
    }
}
