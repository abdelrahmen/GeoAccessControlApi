using GeoAccessControlAPI.Repositories.Absrtactions;
using System.Collections.Concurrent;

namespace GeoAccessControlAPI.Repositories;

public class TempBlockedCountriesRepository : ITempBlockedCountriesRepository
{
    private readonly ConcurrentDictionary<string, DateTimeOffset> _tempBlockedCountries = new();

    public bool TryAdd(string countryCode, DateTimeOffset expiryTime)
    {
        return _tempBlockedCountries.TryAdd(countryCode, expiryTime);
    }

    public bool TryRemove(string countryCode) => _tempBlockedCountries.TryRemove(countryCode, out _);
    public bool Contains(string countryCode) => _tempBlockedCountries.ContainsKey(countryCode);
    public IEnumerable<KeyValuePair<string, DateTimeOffset>> GetAll() => _tempBlockedCountries;

}
