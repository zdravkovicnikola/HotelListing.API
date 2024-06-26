﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListing.API.Data;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;
using HotelListingAPI.Core.Contracts;
using HotelListingAPI.Core.Models.Country;
using HotelListingAPI.Core.Models;
using HotelListingAPI.Core.Exceptions;

namespace HotelListing.API.Controllers
{
    [Route("api/v{version:apiVersion}/countries")]
    [ApiController]
    [ApiVersion("1.0", Deprecated =true)]
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

        // GET: api/Countries/GetAll
        [HttpGet ("GetAll")]
        public async Task<ActionResult<IEnumerable<GetCountryDto>>> GetCountries()
        {
            //select * from Countries
            //return await _context.Countries.ToListAsync();


            //ovo samo po sebi vraca rezultat 201
            //ako bismo hteli da vrati eksplicitno 200 onda bi trebalo da bude
            // return Ok(await _context.Countries.ToListAsync()); -- to Ok()mu obezbedjuje 200 respnse

            // jos jedan mozda pregledniji nacin (u principu ista stvar)


            //kod
            /* 
            var countries = await _countryRepository.GetAllAsync();
            var records = _mapper.Map<List<GetCountryDto>>(countries);
            return Ok(records);
            */

            var countries = await _countryRepository.GetAllAsync<GetCountryDto>();
            return Ok(countries);
        }


        // GET: api/Countries/?StartIndex=0&pagesize=25&PageNumber=1
        [HttpGet]
        public async Task<ActionResult<PagedResult<GetCountryDto>>> GetPagedCountries([FromQuery]QueryParameters queryParameters)
        {
            var pagedCountriesResult = await _countryRepository.GetAllAsync<GetCountryDto>(queryParameters);
            return Ok(pagedCountriesResult);
        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDto>> GetCountry(int id)
        {
            var country = await _countryRepository.GetDetails(id);
            return Ok(country);
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

            /*
            //_context.Entry(country).State = EntityState.Modified;

            var country = await _countryRepository.GetAsync(id);

            if(country == null)
                throw new NotFoundException(nameof(PutCountry), id);

            _mapper.Map(updateCountryDto, country);// bukv country = updateCountryDto
            */
           
            try
            {
                await _countryRepository.UpdateAsync(id, updateCountryDto);
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
        public async Task<ActionResult<CountryDto>> PostCountry(CreateCountyDto createCountryDto)
        {
            /*
            //Klasicna konverzija bez automappera, okej je jer ovde imamo dva polja al da je 10-15 bio bi cim
            //var country = new Country
            //{
            //    Name = createCountryDto.Name,
            //    ShortName = createCountryDto.ShortName,
            //};

            //Konverzija AutoMapperom :O xd
            var country = _mapper.Map<Country>(createCountryDto);
            */

            var country =  await _countryRepository.AddAsync<CreateCountyDto, GetCountryDto>(createCountryDto);
            return CreatedAtAction(nameof(GetCountry), new { id = country.Id }, country);
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            await _countryRepository.DeleteAsync(id);
            return NoContent();
        }

        private async Task<bool> CountryExists(int id)
        {
            return  await _countryRepository.Exists(id);
        }
    }
}
