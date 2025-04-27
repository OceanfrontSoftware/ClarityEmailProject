using EmailService.DataObjects;

namespace EmailService.BusinessObjects
{
    public class MessageStatus
    {
        public Message Message { get; set; }

        public SendAttempt SendAttempts { get; set; }
    }
}
