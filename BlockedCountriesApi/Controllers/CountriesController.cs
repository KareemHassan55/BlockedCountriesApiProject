using BlockedCountriesApi.Models;
using BlockedCountriesApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlockedCountriesApi.Controllers
{
    [ApiController]
    [Route("api/countries")]
    public class CountriesController : ControllerBase
    {
        private readonly CountryService _countryService;

        public CountriesController(CountryService countryService)
        {
            _countryService = countryService;
        }

         [HttpPost("block")]
        public IActionResult BlockCountry([FromBody] CountryBlockRequest request)
        {
            var result = _countryService.BlockCountry(request.CountryCode, request.CountryCode);  
            if (!result)
                return Conflict("Country is already blocked.");

            return Ok("Country blocked successfully.");
        }

         [HttpDelete("block/{countryCode}")]
        public IActionResult UnblockCountry(string countryCode)
        {
            var result = _countryService.UnblockCountry(countryCode);
            if (!result)
                return NotFound("Country not found in block list.");

            return Ok("Country unblocked successfully.");
        }

         [HttpGet("blocked")]
        public IActionResult GetBlockedCountries([FromQuery] string? search = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var countries = _countryService.GetAllBlockedCountries(search);
            var total = countries.Count();

            var paged = countries
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            return Ok(new
            {
                TotalCount = total,
                Page = page,
                PageSize = pageSize,
                Items = paged
            });
        }

         [HttpPost("temporal-block")]
        public IActionResult TemporalBlock([FromBody] TemporalBlockRequest request)
        {
            var result = _countryService.BlockCountryTemporarily(request.CountryCode, request.CountryCode, request.DurationMinutes); // نستخدم CountryCode مؤقتًا كاسم

            if (!result)
                return Conflict("Country is already temporarily blocked.");

            return Ok("Country temporarily blocked.");
        }
    }
}
