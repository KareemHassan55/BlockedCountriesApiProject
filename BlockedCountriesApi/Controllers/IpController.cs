using BlockedCountriesApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlockedCountriesApi.Controllers
{
    [ApiController]
    [Route("api/ip")]
    public class IpController : ControllerBase
    {
        private readonly IpService _ipService;
        private readonly CountryService _countryService;
        private readonly LoggingService _loggingService;

        public IpController(IpService ipService, CountryService countryService, LoggingService loggingService)
        {
            _ipService = ipService;
            _countryService = countryService;
            _loggingService = loggingService;
        }

         [HttpGet("lookup")]
        public async Task<IActionResult> Lookup([FromQuery] string? ipAddress)
        {
            var ip = string.IsNullOrWhiteSpace(ipAddress)
                ? HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown"
                : ipAddress;

            var info = await _ipService.LookupIpAsync(ip);
            if (info == null)
                return BadRequest("Failed to retrieve IP information.");

            return Ok(info);
        }

         [HttpGet("check-block")]
        public async Task<IActionResult> CheckBlock()
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            var userAgent = Request.Headers["User-Agent"].ToString();

            var info = await _ipService.LookupIpAsync(ip);
            if (info == null)
                return BadRequest("Could not fetch IP details.");

            var isBlocked = _countryService.GetAllBlockedCountries()
                .Any(c => c.CountryCode.Equals(info.CountryCode, StringComparison.OrdinalIgnoreCase));

            // Log attempt
            _loggingService.LogAttempt(ip, info.CountryCode, info.Country, isBlocked, userAgent);

            return Ok(new
            {
                Ip = ip,
                CountryCode = info.CountryCode,
                Country = info.Country,
                IsBlocked = isBlocked
            });
        }
    }
}
