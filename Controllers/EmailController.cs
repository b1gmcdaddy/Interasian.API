using Interasian.API.DTOs;
using Interasian.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Interasian.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public EmailController(IEmailService emailService, IConfiguration configuration)
        {
            _emailService = emailService;
            _configuration = configuration;
        }

        [HttpPost("contact")]
        public async Task<IActionResult> SendContactEmail([FromBody] ContactFormDTO contactForm)
        {
            try
            {
                // UI of email content
                var htmlContent = $@"
                    <h2>New Contact Form Submission</h2>
                    <p><strong>Name:</strong> {contactForm.Name}</p>
                    <p><strong>Email:</strong> {contactForm.Email}</p>
                    <p><strong>Subject:</strong> {contactForm.Subject}</p>
                    <p><strong>Message:</strong></p>
                    <p>{contactForm.Message}</p>
                ";

                var email = new EmailDTO(
                    new[] { _configuration["EmailSettings:SenderEmail"] }, 
                    $"Contact Form: {contactForm.Subject}",
                    htmlContent
                );

                await _emailService.SendEmailAsync(email);
                return Ok(new { message = "Your message has been sent successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to send email", error = ex.Message });
            }
        }
    }
} 