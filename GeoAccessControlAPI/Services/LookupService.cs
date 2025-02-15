using GeoAccessControlAPI.Models;
using GeoAccessControlAPI.Models.ResponseModels;
using GeoAccessControlAPI.Services.Abstractions;
using Newtonsoft.Json;
using System.Net;
using System.Text.RegularExpressions;

namespace GeoAccessControlAPI.Services;

public class LookupService : ILookupService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _config;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<LookupService> _logger;

    public LookupService(IHttpClientFactory httpClientFactory, ILogger<LookupService> logger, IHttpContextAccessor httpContextAccessor, IConfiguration config)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _config = config;
    }

    public async Task<ResponseModel<IpInfo>> GetCountryByIpAsync(string ipAddress)
    {
        try
        {
            if (string.IsNullOrEmpty(ipAddress))
                ipAddress = _httpContextAccessor.HttpContext!.Connection.RemoteIpAddress?.ToString()!;

            if (!IPAddress.TryParse(ipAddress, out _))
                return ResponseModel<IpInfo>.Error("Invalid IP address format.");

            var httpClient = _httpClientFactory.CreateClient();

            string url = $"{_config["IpGeolocation:BaseUrl"]}?apiKey={_config["IpGeolocation:ApiKey"]}&ip={ipAddress}";
            var response = await httpClient.GetStringAsync(url);
            var ipInfo = JsonConvert.DeserializeObject<IpInfo>(response);

            if (ipInfo == null || string.IsNullOrEmpty(ipInfo.CountryCode))
                return ResponseModel<IpInfo>.Error("Invalid response from IP lookup service.");

            return ResponseModel<IpInfo>.SuccessResponse(ipInfo);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error fetching IP details: {ex.Message}");
            return ResponseModel<IpInfo>.Error("Error fetching IP details");
        }
    }
}

