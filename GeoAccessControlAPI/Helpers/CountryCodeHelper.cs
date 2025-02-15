using System.Globalization;

namespace GeoAccessControlAPI.Helpers;

public class CountryCodeHelper
{
    public static bool IsValidCountryCode(string countryCode)
    {
        if (string.IsNullOrWhiteSpace(countryCode) || countryCode.Length != 2)
            return false;

        try
        {
            var region = new RegionInfo(countryCode.ToUpper());
            return true;
        }
        catch (ArgumentException)
        {
            return false;
        }
    }
}
