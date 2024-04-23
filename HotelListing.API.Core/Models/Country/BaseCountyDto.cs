using System.ComponentModel.DataAnnotations;

namespace HotelListingAPI.Core.Models.Country
{
    public abstract class BaseCountyDto
    {
        [Required]
        public string Name { get; set; }
        public string ShortName { get; set; }
    }
}
