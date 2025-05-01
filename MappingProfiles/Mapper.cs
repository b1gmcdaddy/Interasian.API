using AutoMapper;
using Interasian.API.DTOs;
using Interasian.API.Models;

namespace Interasian.API.MappingProfiles
{
	public class Mapper : Profile
	{
		public Mapper()
		{
			CreateMap<Listing, ListingDTO>().ReverseMap();
			CreateMap<Listing, CreateListingDTO>().ReverseMap();
		}
	}
}
