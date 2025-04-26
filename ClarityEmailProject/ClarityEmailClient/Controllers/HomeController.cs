using System.Diagnostics;
using ClarityEmailProject.Models;
using EmailService.DataObjects;
using EmailService.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClarityEmailProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IQueuedEmailService _emailService;

        public HomeController(ILogger<HomeController> logger, IQueuedEmailService emailService)
        {
            _logger = logger;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(EmailMessage emailMessage)
        {
            if (!ModelState.IsValid)
            {
                return View(emailMessage);
            }

            try
            {
                // Convert EmailMessage to Message
                var message = new Message
                {
                    FromName = emailMessage.FromName,
                    FromEmail = emailMessage.FromEmail,
                    ToName = emailMessage.ToName,
                    ToEmail = emailMessage.ToEmail,
                    Subject = emailMessage.Subject,
                    Body = emailMessage.Body
                };

                // Send the email
                var messageId = await _emailService.AddMessageToQueue(message);

                // Log success
                _logger.LogInformation($"Email sent successfully. Message ID: {messageId}");

                // Show success message
                TempData["SuccessMessage"] = "Email sent successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log error
                _logger.LogError(ex, "Error sending email");
                
                // Add error to model state
                ModelState.AddModelError("", "An error occurred while sending the email. Please try again later.");
                return View(emailMessage);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
