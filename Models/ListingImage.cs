using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Interasian.API.Models
{
	public class ListingImage
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; } = string.Empty;
		
		[Required]
		public string ListingId { get; set; } = string.Empty;
		[Required]
		public string FileName { get; set; } = string.Empty;
		[Required]
		public DateTime UploadDate { get; set; }
	}
}
