namespace EmailService
{
    public class EmailSettings
    {
        public string SmtpKey { get; set; }
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public int MaxSendAttempts { get; set; }
    }
}
