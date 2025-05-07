namespace Interasian.API.DTOs
{
    public class EmailDTO(IEnumerable<string> to, string subject, string content)
    {
        public IEnumerable<string> To {get; init;} = to;
        public string Subject {get; init;} = subject;
        public string Content {get; init;} = content;
    }

        public class ContactFormDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}