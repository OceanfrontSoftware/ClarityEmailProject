namespace EmailService.DataObjects
{
    public class SendAttempt
    {
        public int Id { get; set; }
        public int MessageId { get; set; }
        public DateTime? DateTime { get; set; }
        public bool Successful { get; set; }
        public string? ErrorMessage { get; set; }
        public int SendAttempts { get; set; }
    }
}
