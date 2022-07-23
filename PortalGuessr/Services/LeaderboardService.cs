using Microsoft.EntityFrameworkCore;
using PortalGuessr.Database;
using PortalGuessr.Database.Entities;

namespace PortalGuessr.Services;

public class LeaderboardService
{
    private readonly DatabaseContext _db;

    public LeaderboardService(DatabaseContext db)
    {
        _db = db;
    }

    public async Task CreateEntry(string playerName, long score)
    {
        var entry = await _db.Leaderboard.FirstOrDefaultAsync(x => x.PlayerName.ToLower() == playerName.ToLower());

        if (entry is null)
        {
            entry = new LeaderboardEntry
            {
                Score = score,
                PlayerName = playerName,
            };

            _db.Leaderboard.Add(entry);
        }
        else
        {
            entry.PlayerName = playerName;
            entry.Score = score;
        }

        await _db.SaveChangesAsync();
    }
}
