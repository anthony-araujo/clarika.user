
using AutoMapper;
using System.Linq;
using ClarikaAppService.Domain.Entities;
using ClarikaAppService.Dto;


namespace ClarikaAppService.Configuration.AutoMapper
{

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {

            CreateMap<UserApp, UserAppDto>()
                .ForMember(dest => dest.CountryId, opt => opt.MapFrom(src => src.CountryId ?? null))
                .ReverseMap();
            CreateMap<Country, CountryDto>().ReverseMap();
            CreateMap<State, StateDto>().ReverseMap();
            CreateMap<City, CityDto>().ReverseMap();
            CreateMap<LocationType, LocationTypeDto>().ReverseMap();
            CreateMap<UserLocation, UserLocationDto>().ReverseMap();
        }
    }
}
