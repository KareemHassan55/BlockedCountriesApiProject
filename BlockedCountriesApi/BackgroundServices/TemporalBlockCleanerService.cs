using BlockedCountriesApi.Services;

namespace BlockedCountriesApi.BackgroundServices
{
    public class TemporalBlockCleanerService : BackgroundService
    {
        private readonly CountryService _countryService;
        private readonly ILogger<TemporalBlockCleanerService> _logger;
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(5);

        public TemporalBlockCleanerService(
            CountryService countryService,
            ILogger<TemporalBlockCleanerService> logger)
        {
            _countryService = countryService;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Temporal Block Cleaner Service started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Cleaning up expired temporary blocks...");
                    _countryService.CleanUpExpiredTemporaryBlocks();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while cleaning temporary blocks");
                }

                await Task.Delay(_interval, stoppingToken);
            }

            _logger.LogInformation("Temporal Block Cleaner Service stopped.");
        }
    }
}
