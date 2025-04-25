using System.ComponentModel.DataAnnotations;

namespace ClarityEmailProject.Models
{
    public class EmailMessage
    {
        [Required(ErrorMessage = "Sender name is required")]
        [Display(Name = "Sender Name")]
        public string FromName { get; set; }

        [Required(ErrorMessage = "Sender email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Sender Email")]
        public string FromEmail { get; set; }

        [Required(ErrorMessage = "Recipient name is required")]
        [Display(Name = "Recipient Name")]
        public string ToName { get; set; }

        [Required(ErrorMessage = "Recipient email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Recipient Email")]
        public string ToEmail { get; set; }

        [Required(ErrorMessage = "Subject is required")]
        [Display(Name = "Subject")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Message body is required")]
        [Display(Name = "Message")]
        public string Body { get; set; }
    }
}
