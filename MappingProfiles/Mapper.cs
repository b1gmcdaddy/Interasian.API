using AutoMapper;
using Interasian.API.DTOs;
using Interasian.API.Models;

namespace Interasian.API.MappingProfiles
{
	public class Mapper : Profile
	{
		public Mapper()
		{
			CreateMap<Listing, ListingDTO>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
				.ReverseMap();
			CreateMap<Listing, CreateListingDTO>().ReverseMap();

			CreateMap<ListingImage, ListingImageDTO>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
				.ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.FileName))
				.ForMember(dest => dest.ListingId, opt => opt.MapFrom(src => src.ListingId))
				.ForMember(dest => dest.UploadDate, opt => opt.MapFrom(src => src.UploadDate));

			CreateMap<ListingImageDTO, ListingImage>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
				.ForMember(dest => dest.ListingId, opt => opt.MapFrom(src => src.ListingId))
				.ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.FileName))
				.ForMember(dest => dest.UploadDate, opt => opt.MapFrom(src => src.UploadDate));
		}
	}
}
