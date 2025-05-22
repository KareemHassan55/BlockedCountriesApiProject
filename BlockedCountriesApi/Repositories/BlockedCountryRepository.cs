using BlockedCountriesApi.Models;
using System.Collections.Concurrent;

namespace BlockedCountriesApi.Repositories
{
    public class BlockedCountryRepository
    {
        private readonly ConcurrentDictionary<string, BlockedCountry> _blockedCountries = new(StringComparer.OrdinalIgnoreCase);
        private readonly List<BlockedAttemptLog> _blockedLogs = new();
        private readonly object _logLock = new();

        public IEnumerable<BlockedCountry> GetAll() => _blockedCountries.Values;

        public bool Add(BlockedCountry country)
        {
            return _blockedCountries.TryAdd(country.CountryCode, country);
        }

        public bool Remove(string countryCode)
        {
            return _blockedCountries.TryRemove(countryCode, out _);
        }

        public bool IsBlocked(string countryCode)
        {
            if (_blockedCountries.TryGetValue(countryCode, out var country))
            {
                if (country.IsTemporary && country.ExpirationTime <= DateTime.UtcNow)
                {
                    
                    _blockedCountries.TryRemove(countryCode, out _);
                    return false;
                }

                return true;
            }

            return false;
        }

        public BlockedCountry? Get(string countryCode)
        {
            _blockedCountries.TryGetValue(countryCode, out var country);
            return country;
        }

        
        public void LogAttempt(BlockedAttemptLog log)
        {
            lock (_logLock)
            {
                _blockedLogs.Add(log);
            }
        }

        public List<BlockedAttemptLog> GetLogs()
        {
            lock (_logLock)
            {
                return _blockedLogs.ToList();
            }
        }

        
        public IEnumerable<BlockedCountry> GetExpiredTemporalBlocks()
        {
            return _blockedCountries.Values
                .Where(c => c.IsTemporary && c.ExpirationTime <= DateTime.UtcNow)
                .ToList();
        }

        public void RemoveExpiredTemporalBlocks()
        {
            foreach (var country in GetExpiredTemporalBlocks())
            {
                _blockedCountries.TryRemove(country.CountryCode, out _);
            }
        }
    }
}
