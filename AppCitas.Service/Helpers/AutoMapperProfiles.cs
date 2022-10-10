using AppCitas.Service.DTOs;
using AppCitas.Service.Entities;
using AutoMapper;

namespace AppCitas.Service.Helpers;

public class AutoMapperProfiles : Profile 
{
	public AutoMapperProfiles()
	{
		CreateMap<AppUser, MemberDto>()
			.ForMember(
				dest => dest.PhotoUrl, 
				opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url))
			.ForMember( dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalcullateAge()))
		CreateMap<Photo, PhotoDto>();
		CreateMap<MemberUpdateDto, AppUser>();
	}
}
