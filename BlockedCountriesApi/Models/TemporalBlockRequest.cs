using System.ComponentModel.DataAnnotations;

namespace BlockedCountriesApi.Models
{
    public class TemporalBlockRequest
    {
        [Required]
        public string CountryCode { get; set; } = default!;

        [Range(1, 1440)]
        public int DurationMinutes { get; set; }
    }
}
