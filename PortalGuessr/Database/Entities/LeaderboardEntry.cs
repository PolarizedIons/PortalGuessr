using PortalGuessr.Database.Entities.Abstractions;

namespace PortalGuessr.Database.Entities;

public class LeaderboardEntry : DbEntity
{
    public string PlayerName { get; set; } = null!;
    public long Score { get; set; }
}
