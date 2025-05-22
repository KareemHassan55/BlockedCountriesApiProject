namespace BlockedCountriesApi.Models
{
    public class BlockedCountry
    {
        public string CountryCode { get; set; } = default!;
        public string CountryName { get; set; } = default!;
        public bool IsTemporary { get; set; } = false;
        public DateTime? ExpirationTime { get; set; } = null;
    }
}
