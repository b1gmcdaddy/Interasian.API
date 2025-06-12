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
                    <!DOCTYPE html>
                    <html>
                    <head>
                        <style>
                            body {{
                                font-family: Arial, sans-serif;
                                line-height: 1.6;
                                color: #333333;
                                margin: 0;
                                padding: 0;
                            }}
                            .email-container {{
                                max-width: 600px;
                                margin: 0 auto;
                                padding: 20px;
                            }}
                            .header {{
                                text-align: center;
                                padding: 20px 0;
                                border-bottom: 2px solid #f0f0f0;
                            }}
                            .logo {{
                                max-width: 200px;
                                height: auto;
                            }}
                            .content {{
                                padding: 20px 0;
                            }}
                            .message-box {{
                                background-color: #f9f9f9;
                                border-left: 4px solid #4a90e2;
                                padding: 15px;
                                margin: 15px 0;
                            }}
                            .footer {{
                                text-align: center;
                                padding: 20px 0;
                                border-top: 2px solid #f0f0f0;
                                font-size: 12px;
                                color: #666666;
                            }}
                        </style>
                    </head>
                    <body>
                        <div class='email-container'>
                            <div class='header'>
                                <img src='https://interasianrealty.vercel.app/logo.png' alt='Inter Asian Realty Services Inc.' class='logo'>
                                <h2 style='color: #2c3e50; margin-top: 15px;'>New Contact Form Submission</h2>
                            </div>
                            <div class='content'>
                                <p><strong>Name:</strong> {contactForm.Name}</p>
                                <p><strong>Email:</strong> {contactForm.Email}</p>
                                <p><strong>Subject:</strong> {contactForm.Subject}</p>
                                <p><strong>Phone:</strong> {contactForm.Phone}</p>
                                <div class='message-box'>
                                    <p><strong>Message:</strong></p>
                                    <p>{contactForm.Message}</p>
                                </div>
                            </div>
                            <div class='footer'>
                                <p>This email was sent from the contact form on Inter Asian Realty Services Inc. website.</p>
                                <p>© {DateTime.Now.Year} Inter Asian Realty Services Inc. All rights reserved.</p>
                            </div>
                        </div>
                    </body>
                    </html>
                ";

                var email = new EmailDTO(
                    new[] { _configuration["EmailSettings:SenderEmail"] }, 
                    $"Inter Asian Realty Services Inc. - Contact Form Submission: {contactForm.Subject}",
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