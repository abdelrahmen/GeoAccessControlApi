# GeoAccessControlApi

# Try the Demo At: http://geoaccesscontrolapi.runasp.net/swagger/index.html
 
## Main System Components

### Repositories

- **BlockedCountriesRepository** (IBlockedCountriesRepository)
  - Manages permanently blocked countries
  - Singleton implementation

- **TempBlockedCountriesRepository** (ITempBlockedCountriesRepository)
  - Handles temporarily blocked countries
  - Singleton implementation

- **BlockedAttemptsRepository** (IBlockedAttemptsRepository)
  - Tracks blocked access attempts
  - Singleton implementation

### Services

- **BlockService** (IBlockService)
  - Manages blocking logic
  - Scoped implementation

- **LookupService** (ILookupService)
  - Handles geographical lookups
  - Scoped implementation

- **TempBlockCleanupService**
  - Background service for cleanup operations
  - Hosted service implementation