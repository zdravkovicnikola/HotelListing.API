using HotelListing.API.Data;
using HotelListingAPI.Core.Models;
using HotelListingAPI.Core.Models.Country;

namespace HotelListingAPI.Core.Contracts
{
    public interface ICountryRepository : IGenericRepository<Country>
    {
        Task<CountryDto> GetDetails(int id);

    }
}
