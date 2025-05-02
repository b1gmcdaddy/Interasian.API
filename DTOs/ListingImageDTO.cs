namespace Interasian.API.DTOs
{
    public class ListingImageDTO
    {
        public int ImageId {get; set;}
        public int ListingId { get; set; }
        public string FileName {get; set;} = string.Empty;
        public DateTime UploadDate {get; set;}
        public string ImageUrl { get; set; } = string.Empty;
    }
}