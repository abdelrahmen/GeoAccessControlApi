using GeoAccessControlAPI.Models;

namespace GeoAccessControlAPI.Repositories.Absrtactions;

public interface IBlockedAttemptsRepository
{
    void Add(BlockedAttemptLog log);
    IEnumerable<BlockedAttemptLog> GetAll();
    int Count();
}
