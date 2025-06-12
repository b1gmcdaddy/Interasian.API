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

			CreateMap<ListingImage, ListingImageDTO>()
				.ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.FileName))
				.ForMember(dest => dest.ListingId, opt => opt.MapFrom(src => src.ListingId))
				.ForMember(dest => dest.ImageId, opt => opt.MapFrom(src => src.ImageId))
				.ForMember(dest => dest.UploadDate, opt => opt.MapFrom(src => src.UploadDate));

			CreateMap<ListingImageDTO, ListingImage>()
				.ForMember(dest => dest.ListingId, opt => opt.MapFrom(src => src.ListingId))
				.ForMember(dest => dest.ImageId, opt => opt.MapFrom(src => src.ImageId))
				.ForMember(dest => dest.UploadDate, opt => opt.MapFrom(src => src.UploadDate))
				.ForMember(dest => dest.Listing, opt => opt.Ignore());
		}
	}
}
