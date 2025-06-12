using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Interasian.API.Models
{
	public class ListingImage
	{
		[Key]
		public int ImageId { get; set; }
		[Required]
		public int ListingId { get; set; }
		[Required]
		public string FileName { get; set; } = string.Empty;
		[Required]
		public DateTime UploadDate { get; set; }

		[ForeignKey("ListingId")]
		public Listing? Listing { get; set; }
	}
}
