using GeoAccessControlAPI.Helpers;
using GeoAccessControlAPI.Models;
using GeoAccessControlAPI.Models.RequestModels;
using GeoAccessControlAPI.Models.ResponseModels;
using GeoAccessControlAPI.Repositories.Absrtactions;
using GeoAccessControlAPI.Services.Abstractions;

namespace GeoAccessControlAPI.Services;

public class BlockService : IBlockService
{
    private readonly IBlockedCountriesRepository _blockedCountriesRepository;
    private readonly ITempBlockedCountriesRepository _tempBlockedCountriesRepository;
    private readonly IBlockedAttemptsRepository _blockedAttemptsRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILookupService _ipLookupService;

    public BlockService(IHttpContextAccessor httpContextAccessor, ILookupService ipLookupService, IBlockedCountriesRepository blockedCountriesRepository, ITempBlockedCountriesRepository tempBlockedCountriesRepository, IBlockedAttemptsRepository blockedAttemptsRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _ipLookupService = ipLookupService;
        _blockedCountriesRepository = blockedCountriesRepository;
        _tempBlockedCountriesRepository = tempBlockedCountriesRepository;
        _blockedAttemptsRepository = blockedAttemptsRepository;
    }

    public ResponseModel<string> BlockCountry(string countryCode)
    {
        if (!CountryCodeHelper.IsValidCountryCode(countryCode))
            return ResponseModel<string>.Error("Invalid country code");

        if (_blockedCountriesRepository.TryAdd(countryCode))
            return ResponseModel<string>.SuccessResponse($"{countryCode} blocked successfully.");
        else
            return ResponseModel<string>.Error($"{countryCode} is already blocked.");
    }

    public ResponseModel<string> UnblockCountry(string countryCode)
    {
        if (_blockedCountriesRepository.TryRemove(countryCode))
            return ResponseModel<string>.SuccessResponse($"{countryCode} unblocked successfully.");

        return ResponseModel<string>.Error($"{countryCode} is not in the blocked list.");
    }

    public async Task<ResponseModel<bool>> IsBlocked()
    {
        var ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString();
        var getCountryRequest = await _ipLookupService.GetCountryByIpAsync(ipAddress);

        if (!getCountryRequest.Success)
            return ResponseModel<bool>.Error("Couldn't retrieve country code");

        var countryCode = getCountryRequest.Data.CountryCode;
        var isBlocked = _blockedCountriesRepository.Contains(countryCode)
            || _tempBlockedCountriesRepository.Contains(countryCode);

        LogBlockedAttempt(ipAddress, countryCode, isBlocked,
            _httpContextAccessor.HttpContext.Request.Headers["User-Agent"].ToString());


        return ResponseModel<bool>.SuccessResponse(isBlocked, $"your country block status is: {isBlocked}");
    }

    public bool TryBlockCountry(string countryCode, double durationMinutes, out string message)
    {
        if (_tempBlockedCountriesRepository.Contains(countryCode))
        {
            message = $"Country {countryCode} is already temporarily blocked.";
            return false;
        }

        DateTime expiryTime = DateTime.UtcNow.AddMinutes(durationMinutes);
        _tempBlockedCountriesRepository.TryAdd(countryCode, expiryTime);
        message = $"Country {countryCode} blocked for {durationMinutes} minutes.";
        return true;
    }

    private void LogBlockedAttempt(string ipAddress, string countryCode, bool isBlocked, string userAgent)
    {
        _blockedAttemptsRepository.Add(new BlockedAttemptLog
        {
            IPAddress = ipAddress,
            CountryCode = countryCode,
            Blocked = isBlocked,
            Timestamp = DateTime.UtcNow,
            UserAgent = userAgent
        });
    }

    public PaginatedResponseModel<IEnumerable<BlockedAttemptLog>> GetBlockedLogs(PaginationRequestModel request)
    {
        var allLogs = _blockedAttemptsRepository.GetAll();
        return new PaginatedResponseModel<IEnumerable<BlockedAttemptLog>>
        {
            Success = true,
            TotalRecords = _blockedAttemptsRepository.Count(),
            PageNumber = request.Page,
            PageSize = request.PageSize,
            Data = allLogs.OrderByDescending(log => log.Timestamp)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList(),
        };
    }

    public PaginatedResponseModel<IEnumerable<string>> GetBlockedCountries(PaginationRequestModel request)
    {
        var blockedList = _blockedCountriesRepository.GetAll();

        if (!string.IsNullOrEmpty(request.Search))
            blockedList = blockedList.Where(c => c.Contains(request.Search, StringComparison.OrdinalIgnoreCase));

        var pagedResult = blockedList
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        return new PaginatedResponseModel<IEnumerable<string>>
        {
            Success = true,
            TotalRecords = pagedResult.Count,
            PageNumber = request.Page,
            PageSize = request.PageSize,
            Data = pagedResult,
        };
    }
}
