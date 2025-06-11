using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Interasian.API.DTOs
{
	public class CreateListingDTO
	{
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
		[Required]
		public string PropertyType {get; set;} = string.Empty;
		public string Owner {get; set;} = string.Empty;
		public string Creator {get; set;} = string.Empty;
	}

	public class ListingDTO : CreateListingDTO
	{
		public int ListingId { get; set; }
		public List<ListingImageDTO>? Images { get; set; } = new List<ListingImageDTO>();
	}
}
