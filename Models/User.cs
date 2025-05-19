using Microsoft.AspNetCore.Identity;

namespace Interasian.API.Models
{
	public class User : IdentityUser
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
	}
}
