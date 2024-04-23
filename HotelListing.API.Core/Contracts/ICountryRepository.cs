using HotelListing.API.Data;
using HotelListingAPI.Core.Models;

namespace HotelListingAPI.Core.Contracts
{
    public interface ICountryRepository : IGenericRepository<Country>
    {
        Task<Country> GetDetails(int id);
        Task<PagedResult<TResult>> GetAllAsync<TResult>(QueryParameters queryParameters);

    }
}
