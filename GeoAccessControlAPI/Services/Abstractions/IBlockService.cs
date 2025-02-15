using GeoAccessControlAPI.Models.RequestModels;
using GeoAccessControlAPI.Models;
using GeoAccessControlAPI.Models.ResponseModels;

namespace GeoAccessControlAPI.Services.Abstractions;

public interface IBlockService
{
    ResponseModel<string> BlockCountry(string countryCode);
    ResponseModel<string> UnblockCountry(string countryCode);
    Task<ResponseModel<bool>> IsBlocked();
    bool TryBlockCountry(string countryCode, double durationMinutes, out string message);
    PaginatedResponseModel<IEnumerable<BlockedAttemptLog>> GetBlockedLogs(PaginationRequestModel request);
    PaginatedResponseModel<IEnumerable<string>> GetBlockedCountries(PaginationRequestModel request);
}
