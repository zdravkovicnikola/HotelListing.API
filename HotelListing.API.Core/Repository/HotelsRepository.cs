using AutoMapper;
using HotelListingAPI.Core.Contracts;
using HotelListing.API.Data;

namespace HotelListingAPI.Core.Repository
{
    public class HotelsRepository : GenericRepository<Hotel>, IHotelsRepository
    {
        public HotelsRepository(HotelListingDbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
