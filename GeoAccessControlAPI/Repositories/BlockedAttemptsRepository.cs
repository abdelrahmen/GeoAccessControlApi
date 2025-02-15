using GeoAccessControlAPI.Models;
using GeoAccessControlAPI.Repositories.Absrtactions;
using System.Collections.Concurrent;

namespace GeoAccessControlAPI.Repositories;

public class BlockedAttemptsRepository : IBlockedAttemptsRepository
{
    private readonly ConcurrentBag<BlockedAttemptLog> _blockedAttempts = new();

    public void Add(BlockedAttemptLog log) => _blockedAttempts.Add(log);
    public IEnumerable<BlockedAttemptLog> GetAll() => _blockedAttempts;
    public int Count() => _blockedAttempts.Count;
}
