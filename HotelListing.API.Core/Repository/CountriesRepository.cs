﻿using AutoMapper;
using HotelListingAPI.Core.Contracts;
using HotelListing.API.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelListingAPI.Core.Repository
{
    public class CountriesRepository : GenericRepository<Country>, ICountryRepository
    {
        private readonly HotelListingDbContext _context;

        public CountriesRepository(HotelListingDbContext context, IMapper mapper) : base(context, mapper)
        {
            this._context = context;
        }

        public async Task<Country> GetDetails(int id)
        {
            return await _context.Countries.Include(q=>q.Hotels).FirstOrDefaultAsync(q => q.Id == id);
        }
    }
}
