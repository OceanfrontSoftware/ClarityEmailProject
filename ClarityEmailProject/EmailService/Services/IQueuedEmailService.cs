using EmailService.DataObjects;

namespace EmailService.Services
{
    public interface IQueuedEmailService
    {
        public Task<int> AddMessageToQueue(Message message);

        public Task ProcessMessageQueueAsync();
    }
} 