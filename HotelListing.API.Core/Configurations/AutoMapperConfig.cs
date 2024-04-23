using AutoMapper;
using HotelListing.API.Data;
using HotelListingAPI.Core.Models.Country;
using HotelListingAPI.Core.Models.Hotel;
using HotelListingAPI.Core.Models.Users;

namespace HotelListingAPI.Core.Configurations
{
    public class AutoMapperConfig:Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Country, CreateCountyDto>().ReverseMap();
            CreateMap<Country, GetCountryDto>().ReverseMap();
            CreateMap<Country, CountryDto>().ReverseMap();
            CreateMap<Country, UpdateCountryDto>().ReverseMap();


            CreateMap<Hotel, HotelDto>().ReverseMap();
            CreateMap<Hotel, CreateHotelDto>().ReverseMap();
            CreateMap<Hotel, UpdateHotelDto>().ReverseMap();

            CreateMap<ApiUserDto, ApiUser>().ReverseMap();
        }

    }
}
