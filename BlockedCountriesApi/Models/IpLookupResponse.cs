namespace BlockedCountriesApi.Models
{
    public class IpLookupResponse
    {
        public string Ip { get; set; } = default!;
        public string Country { get; set; } = default!;
        public string CountryCode { get; set; } = default!;
        public string Org { get; set; } = default!;
    }
}
