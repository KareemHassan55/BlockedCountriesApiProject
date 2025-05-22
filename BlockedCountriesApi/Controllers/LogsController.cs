using BlockedCountriesApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlockedCountriesApi.Controllers
{
    [ApiController]
    [Route("api/logs")]
    public class LogsController : ControllerBase
    {
        private readonly LoggingService _loggingService;

        public LogsController(LoggingService loggingService)
        {
            _loggingService = loggingService;
        }

         [HttpGet("blocked-attempts")]
        public IActionResult GetBlockedAttempts([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var logs = _loggingService.GetAllLogs(page, pageSize);

            return Ok(new
            {
                Page = page,
                PageSize = pageSize,
                Items = logs
            });
        }
    }
}
