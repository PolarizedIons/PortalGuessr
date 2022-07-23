using FastEndpoints;
using PortalGuessr.Services;

namespace PortalGuessr.Features.Leaderboard.GetLeaderboard;

public class Endpoint : EndpointWithoutRequest<Response>
{
    private readonly GameService _gameService;

    public Endpoint(GameService gameService)
    {
        _gameService = gameService;
    }

    public override void Configure()
    {
        Verbs(Http.GET);
        Routes("/leaderboard");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var leaderboard = await _gameService.GetLeaderboard();

        Response.Items = leaderboard.Select(x => new ResponseItem
        {
            Score = x.Score,
            PlayerName = x.PlayerName,
        });
    }
}
