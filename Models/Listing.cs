using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Interasian.API.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Interasian.API.Models
{
	public class Listing
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; } = string.Empty;
		
		[Required]
		public string Title { get; set; } = string.Empty;
		[Required]
		public string Location { get; set; } = string.Empty;
		public string? LandArea { get; set; } = string.Empty;
		public string? FloorArea { get; set; } = string.Empty;
		public int? BedRooms { get; set; }
		public int? BathRooms { get; set; }
		
		[BsonRepresentation(BsonType.Decimal128)]
		[Required]
		public decimal Price { get; set; }
		public string? Description { get; set; } = string.Empty;
		[Required]
		public bool Status { get; set; }
		[Required]
		public string PropertyType {get; set;} = string.Empty;
		[Required]
		public string Owner {get; set;} = string.Empty;
		[Required]
		public string Creator {get; set;} = string.Empty;
		
		// Navigation property for image
		public List<string>? ImageIds {get; set;} = new List<string>();
	}
}
