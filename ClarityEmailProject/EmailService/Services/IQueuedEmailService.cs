using EmailService.BusinessObjects;
using EmailService.DataObjects;

namespace EmailService.Services
{
    public interface IQueuedEmailService
    {
        Task<int> AddMessageToQueue(Message message);

        Task ProcessMessageQueueAsync();

        Task<MessageStatus> GetMessageStatus(int messageId);

        Task Retry(int messageId);
    }
} 