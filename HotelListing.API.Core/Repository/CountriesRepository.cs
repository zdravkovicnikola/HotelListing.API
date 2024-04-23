using AutoMapper;
using HotelListingAPI.Core.Contracts;
using HotelListing.API.Data;
using Microsoft.EntityFrameworkCore;
using HotelListingAPI.Core.Exceptions;
using AutoMapper.QueryableExtensions;
using HotelListingAPI.Core.Models.Country;

namespace HotelListingAPI.Core.Repository
{
    public class CountriesRepository : GenericRepository<Country>, ICountryRepository
    {
        private readonly HotelListingDbContext _context;
        private readonly IMapper _mapper;

        public CountriesRepository(HotelListingDbContext context, IMapper mapper) : base(context, mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<CountryDto> GetDetails(int id)
        {
            var country =  await _context.Countries.Include(q=>q.Hotels)
                .ProjectTo<CountryDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(q => q.Id == id);
            if (country is null)
            {
                throw new NotFoundException(nameof(GetDetails), id);
            }
            return country;

        }
    }
}
