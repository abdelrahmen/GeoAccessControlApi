namespace GeoAccessControlAPI.Repositories.Absrtactions;

public interface IBlockedCountriesRepository
{
    bool TryAdd(string countryCode);
    bool TryRemove(string countryCode);
    bool Contains(string countryCode);
    IEnumerable<string> GetAll();
}
