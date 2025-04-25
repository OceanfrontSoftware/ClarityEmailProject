using EmailService.DataObjects;
using Microsoft.EntityFrameworkCore;

namespace EmailService.Services
{
    public class EmailService
    {
        private readonly EmailDbContext _dbContext;

        public EmailService(EmailDbContext dbContext)
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
