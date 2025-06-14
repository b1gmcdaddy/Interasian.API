namespace Interasian.API.DTOs
{
    public class ListingImageDTO
    {
        public string Id { get; set; } = string.Empty;
        public string ListingId { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public DateTime UploadDate { get; set; }
    }
}