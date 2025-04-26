namespace EmailService.Services
{
    public class EmailBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<EmailBackgroundService> _logger;

        public EmailBackgroundService(IServiceScopeFactory scopeFactory, ILogger<EmailBackgroundService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Email processor started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    // store the email content in the database
                    var emailService = scope.ServiceProvider.GetRequiredService<IQueuedEmailService>();
                    await emailService.ProcessMessageQueueAsync();
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }

            _logger.LogInformation("Email processor stopped.");
        }
    }
}
