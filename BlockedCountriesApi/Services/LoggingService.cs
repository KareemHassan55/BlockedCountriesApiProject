using BlockedCountriesApi.Models;
using BlockedCountriesApi.Repositories;

namespace BlockedCountriesApi.Services
{
    public class LoggingService
    {
        private readonly BlockedCountryRepository _repository;

        public LoggingService(BlockedCountryRepository repository)
        {
            _repository = repository;
        }

        public void LogAttempt(string ip, string countryCode, string countryName, bool isBlocked, string userAgent)
        {
            var log = new BlockedAttemptLog
            {
                IpAddress = ip,
                CountryCode = countryCode,
                CountryName = countryName,
                IsBlocked = isBlocked,
                Timestamp = DateTime.UtcNow,
                UserAgent = userAgent
            };

            _repository.LogAttempt(log);
        }

        public IEnumerable<BlockedAttemptLog> GetAllLogs(int page = 1, int pageSize = 10)
        {
            return _repository.GetLogs()
                .OrderByDescending(l => l.Timestamp)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }
    }
}
