namespace EmailService.DataObjects
{
    public class SendEmailResponse
    {
        public Message Message { get; set; }
        public DateTime Sent { get; set; }
        public bool Successful { get; set; }
        public Exception? Error { get; set; }
    }
}
