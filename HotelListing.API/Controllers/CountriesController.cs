using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListing.API.Data;
using HotelListing.API.Models.Country;
using AutoMapper;
using HotelListing.API.Models.Hotel;
using HotelListing.API.Contracts;
using Microsoft.AspNetCore.Authorization;

namespace HotelListing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class CountriesController : ControllerBase
    {
        
        private readonly IMapper _mapper;
        private readonly ICountryRepository _countryRepository;
        private readonly ILogger<CountriesController> _logger;

        public CountriesController(IMapper mapper, ICountryRepository countryRepository, ILogger<CountriesController> logger)
        {
            this._mapper = mapper;
            this._countryRepository = countryRepository;
            this._logger = logger;
        }

        // GET: api/Countries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCountryDto>>> GetCountries()
        {
            //select * from Countries
            //return await _context.Countries.ToListAsync();


            //ovo samo po sebi vraca rezultat 201
            //ako bismo hteli da vrati eksplicitno 200 onda bi trebalo da bude
            // return Ok(await _context.Countries.ToListAsync()); -- to Ok()mu obezbedjuje 200 respnse

            // jos jedan mozda pregledniji nacin (u principu ista stvar)

            var countries = await _countryRepository.GetAllAsync();
            var records = _mapper.Map<List<GetCountryDto>>(countries);
            return Ok(records);


        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDto>> GetCountry(int id)
        {
            var country = await _countryRepository.GetDetails(id);

            if (country == null)
            {
                _logger.LogWarning($"Record found in {nameof(GetCountry)} with Id:{id}.");
                return NotFound();
            }
            var record = _mapper.Map<CountryDto>(country);
            return record;
        }

        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutCountry(int id, UpdateCountryDto updateCountryDto)
        {
            if (id != updateCountryDto.Id)
            {
                return BadRequest("Invalid record Id!");
            }

            //_context.Entry(country).State = EntityState.Modified;

            var country = await _countryRepository.GetAsync(id);

            if(country == null)
                return NotFound();

            _mapper.Map(updateCountryDto, country);// bukv country = updateCountryDto

            try
            {
                await _countryRepository.UpdateAsync(country);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CountryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Countries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Country>> PostCountry(CreateCountyDto createCountryDto)
        {
            //Klasicna konverzija bez automappera, okej je jer ovde imamo dva polja al da je 10-15 bio bi cim
            //var country = new Country
            //{
            //    Name = createCountryDto.Name,
            //    ShortName = createCountryDto.ShortName,
            //};

            //Konverzija AutoMapperom :O xd
            var country = _mapper.Map<Country>(createCountryDto);
           
            await _countryRepository.AddAsync(country);

            return CreatedAtAction("GetCountry", new { id = country.Id }, country);
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var country = await _countryRepository.GetAsync(id);
            if (country == null)
            {
                return NotFound();
            }

            await _countryRepository.DeleteAsync(id);

            return NoContent();
        }

        private async Task<bool> CountryExists(int id)
        {
            return  await _countryRepository.Exists(id);
        }
    }
}
