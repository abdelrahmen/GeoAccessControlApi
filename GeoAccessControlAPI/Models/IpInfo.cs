using Newtonsoft.Json;

namespace GeoAccessControlAPI.Models;

public class IpInfo
{
    [JsonProperty("ip")]
    public string Ip { get; set; }

    [JsonProperty("country_name")]
    public string CountryName { get; set; }

    [JsonProperty("country_code2")]
    public string CountryCode { get; set; }

    [JsonProperty("isp")]
    public string Isp { get; set; }
}
