using GeoAccessControlAPI.Models.ResponseModels;
using GeoAccessControlAPI.Models;

namespace GeoAccessControlAPI.Services.Abstractions;

public interface ILookupService
{
    Task<ResponseModel<IpInfo>> GetCountryByIpAsync(string ipAddress);
}
