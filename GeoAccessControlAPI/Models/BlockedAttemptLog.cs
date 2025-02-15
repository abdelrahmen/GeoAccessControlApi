namespace GeoAccessControlAPI.Models;

public class BlockedAttemptLog
{
    public string IPAddress { get; set; }
    public string CountryCode { get; set; }
    public bool Blocked { get; set; }
    public DateTime Timestamp { get; set; }
    public string UserAgent { get; set; }
}