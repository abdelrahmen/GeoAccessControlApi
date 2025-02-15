namespace GeoAccessControlAPI.Models.RequestModels;

public class TemporalBlockRequest
{
    public string CountryCode { get; set; }
    public double DurationMinutes { get; set; }
}
