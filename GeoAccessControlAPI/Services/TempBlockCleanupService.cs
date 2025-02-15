using GeoAccessControlAPI.Repositories.Absrtactions;
using GeoAccessControlAPI.Services.Abstractions;

namespace GeoAccessControlAPI.Services;

public class TempBlockCleanupService : BackgroundService
{
    private readonly ITempBlockedCountriesRepository _tempBlockedCountriesRepository;
    private readonly ILogger<TempBlockCleanupService> _logger;

    public TempBlockCleanupService( ILogger<TempBlockCleanupService> logger, ITempBlockedCountriesRepository tempBlockedCountriesRepository)
    {
        _tempBlockedCountriesRepository = tempBlockedCountriesRepository;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTimeOffset.UtcNow;
            var expiredCountries = _tempBlockedCountriesRepository.GetAll()
                .Where(kvp => kvp.Value <= now)
                .Select(kvp => kvp.Key)
                .ToList();

            foreach (var countryCode in expiredCountries)
                _tempBlockedCountriesRepository.TryRemove(countryCode);

            _logger.LogInformation("Expired temporary blocks removed.");
            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
}
