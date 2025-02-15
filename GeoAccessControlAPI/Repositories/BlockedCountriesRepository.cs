using GeoAccessControlAPI.Repositories.Absrtactions;
using System.Collections.Concurrent;

namespace GeoAccessControlAPI.Repositories;

public class BlockedCountriesRepository : IBlockedCountriesRepository
{
    private readonly ConcurrentDictionary<string, bool> _blockedCountries = new();

    public bool TryAdd(string countryCode) => _blockedCountries.TryAdd(countryCode, true);
    public bool TryRemove(string countryCode) => _blockedCountries.TryRemove(countryCode, out _);
    public bool Contains(string countryCode) => _blockedCountries.ContainsKey(countryCode);
    public IEnumerable<string> GetAll() => _blockedCountries.Keys;
}
