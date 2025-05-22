using BlockedCountriesApi.Models;
using BlockedCountriesApi.Repositories;

namespace BlockedCountriesApi.Services
{
    public class CountryService
    {
        private readonly BlockedCountryRepository _repository;

        public CountryService(BlockedCountryRepository repository)
        {
            _repository = repository;
        }

        public bool BlockCountry(string countryCode, string countryName)
        {
            var country = new BlockedCountry
            {
                CountryCode = countryCode.ToUpper(),
                CountryName = countryName,
                IsTemporary = false,
                ExpirationTime = null
            };

            return _repository.Add(country);
        }

        public bool UnblockCountry(string countryCode)
        {
            return _repository.Remove(countryCode.ToUpper());
        }

        public IEnumerable<BlockedCountry> GetAllBlockedCountries(string? search = null)
        {
            var countries = _repository.GetAll();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();
                countries = countries.Where(c =>
                    c.CountryCode.ToLower().Contains(search) ||
                    c.CountryName.ToLower().Contains(search));
            }

            return countries;
        }

        public bool BlockCountryTemporarily(string countryCode, string countryName, int durationMinutes)
        {
            var existing = _repository.Get(countryCode);
            if (existing != null && existing.IsTemporary)
            {
                return false; 
            }

            var country = new BlockedCountry
            {
                CountryCode = countryCode.ToUpper(),
                CountryName = countryName,
                IsTemporary = true,
                ExpirationTime = DateTime.UtcNow.AddMinutes(durationMinutes)
            };

            return _repository.Add(country);
        }

        public void CleanUpExpiredTemporaryBlocks()
        {
            _repository.RemoveExpiredTemporalBlocks();
        }
    }
}
