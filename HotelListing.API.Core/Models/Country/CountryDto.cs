using HotelListing.API.Data;
using HotelListingAPI.Core.Models.Hotel;

namespace HotelListingAPI.Core.Models.Country
{
    public class CountryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public virtual IList<HotelDto> Hotels { get; set; }
    }
}
