using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Interasian.API.Models
{
	public class Listing
	{
		[Key, Required]
		public int ListingId { get; set; }
		[Required]
		public string Title { get; set; } = string.Empty;
		[Required]
		public string Location { get; set; } = string.Empty;
		public string? LandArea { get; set; } = string.Empty;
		public string? FloorArea { get; set; } = string.Empty;
		public int? BedRooms { get; set; }
		public int? BathRooms { get; set; }
		
		[Column(TypeName = "decimal(18,2)")]
		[Required]
		public decimal Price { get; set; }
		public string? Description { get; set; } = string.Empty;
		[Required]
		public bool Status { get; set; }

		// Navigation property for image
		public ICollection<ListingImage>? Images {get; set;} = new List<ListingImage>();
	}

	public class ListingImage
	{
		[Key]
		public int ImageId {get; set;}
		[Required]
		public int ListingId {get;set;}
		[Required]
		public string FileName {get; set;} = string.Empty;
		[Required]
		public DateTime UploadDate {get; set;}
		
		[ForeignKey("ListingId")]
		public Listing? Listing {get; set;}
	}
}
