namespace GeoAccessControlAPI.Repositories.Absrtactions;

public interface ITempBlockedCountriesRepository
{
    bool TryAdd(string countryCode, DateTimeOffset expiryTime);
    bool TryRemove(string countryCode);
    bool Contains(string countryCode);
    IEnumerable<KeyValuePair<string, DateTimeOffset>> GetAll();
}
