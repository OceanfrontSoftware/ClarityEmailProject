using EmailService.DataObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using EmailService.BusinessObjects;

namespace EmailService.Services
{
    public class QueuedEmailService : IQueuedEmailService
    {
        private readonly EmailDbContext _dbContext;
        private readonly EmailSettings _emailSettings;
        private static bool _processing = false;

        public QueuedEmailService(EmailDbContext dbContext, IOptions<EmailSettings> options)
        {
            _dbContext = dbContext;
            _emailSettings = options.Value;
        }


        /// <summary>
        /// Adds an email messagt to the queue to be processed by a background task
        /// </summary>
        /// <param name="message"></param>
        /// <returns>Id of the email message in the database</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<int> AddMessageToQueue(Message message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            // save the email content
            _dbContext.Messages.Add(message);
            await _dbContext.SaveChangesAsync();

            // add to queue
            SendAttempt attempt = new SendAttempt() { MessageId = message.Id, SendAttempts = 0, Successful = false };
            _dbContext.SendAttempts.Add(attempt);
            await _dbContext.SaveChangesAsync();

            return message.Id;
        }


        /// <summary>
        /// Adds a message to a queue for background processing
        /// </summary>
        /// <returns></returns>
        public async Task ProcessMessageQueueAsync()
        {
            _processing = true;

            // search queue for messages that have not been sent successfully and have less than 3 attempts
            var queue = _dbContext.SendAttempts.Where(a => !a.Successful && a.SendAttempts < _emailSettings.MaxSendAttempts).ToList();

            foreach (var sendAttempt in queue)
            {
                var message = _dbContext.Messages.FirstOrDefault(m => m.Id == sendAttempt.MessageId);

                // ensure the message object exists
                if (message == null)
                    continue;

                try
                {
                    // attempt to send email
                    sendAttempt.SendAttempts++;
                    var response = await SendEmailAsync(message);

                    // increment send attempt and set queue item to successful
                    sendAttempt.Successful = response.Successful;
                    sendAttempt.DateTime = response.Sent;
                    sendAttempt.ErrorMessage = response.Error?.ToString();
                }
                catch (Exception ex)
                {
                    // update item with error message
                    sendAttempt.ErrorMessage += ex.ToString();
                }

                // update the send attempt item
                _dbContext.SendAttempts.Update(sendAttempt);
                await _dbContext.SaveChangesAsync();

            }

            _processing = false;

        }


        /// <summary>
        /// Directly send an smtp email message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<SendEmailResponse> SendEmailAsync(Message message)
        {
            using var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort)
            {
                Credentials = new NetworkCredential(message.FromEmail, _emailSettings.SmtpKey),
                EnableSsl = true
            };

            using var mailMessage = new MailMessage
            {
                From = new MailAddress(message.FromEmail, message.FromName),
                Subject = message.Subject,
                Body = message.Body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(new MailAddress(message.ToEmail, message.ToName));

            SendEmailResponse response = new SendEmailResponse() { Message = message, Sent = DateTime.Now };

            try
            {
                await client.SendMailAsync(mailMessage);
                response.Successful = true;
            }
            catch (Exception ex)
            {
                response.Successful = false;
                response.Error = ex;
            }

            return response;
        }


        /// <summary>
        /// Gets status information for an email message
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public async Task<MessageStatus> GetMessageStatus(int messageId)
        {
            return new MessageStatus()
            {
                Message = await _dbContext.Messages.FindAsync(messageId),
                SendAttempts = await _dbContext.SendAttempts.FirstOrDefaultAsync(s => s.MessageId == messageId)
            };
        }


        /// <summary>
        /// Reset the send attempts so that a failed email will try to be sent again
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public async Task Retry(int messageId)
        {
            var attempt = await _dbContext.SendAttempts.FirstOrDefaultAsync(s => s.MessageId == messageId);
            if (attempt == null)
                return;

            attempt.SendAttempts = 0;
            _dbContext.SendAttempts.Update(attempt);
            await _dbContext.SaveChangesAsync();
        }
    }
}
