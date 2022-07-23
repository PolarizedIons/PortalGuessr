namespace PortalGuessr.Features.Leaderboard.GetLeaderboard;

public class Response
{
    public IEnumerable<ResponseItem> Items { get; set; } = null!;
}

public class ResponseItem
{
    public string PlayerName { get; set; } = null!;
    public long Score { get; set; }
}
