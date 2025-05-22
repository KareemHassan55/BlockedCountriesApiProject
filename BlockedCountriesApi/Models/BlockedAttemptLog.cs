namespace BlockedCountriesApi.Models
{
    public class BlockedAttemptLog
    {
        public string IpAddress { get; set; } = default!;
        public string CountryCode { get; set; } = default!;
        public string CountryName { get; set; } = default!;
        public bool IsBlocked { get; set; }
        public DateTime Timestamp { get; set; }
        public string UserAgent { get; set; } = default!;
    }
}
