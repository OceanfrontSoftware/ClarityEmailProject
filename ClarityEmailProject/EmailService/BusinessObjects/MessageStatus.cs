using EmailService.DataObjects;

namespace EmailService.BusinessObjects
{
    public class MessageStatus
    {
        public Message Message { get; set; }

        public List<SendAttempt> SendAttempts { get; set; } = new List<SendAttempt>();
    }
}
