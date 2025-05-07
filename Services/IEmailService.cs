using Interasian.API.DTOs;

namespace Interasian.API.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailDTO emailDto);
    }
} 