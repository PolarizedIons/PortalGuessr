using FastEndpoints;
using PortalGuessr.Services;

namespace PortalGuessr.Features.Game.Start;

public class Endpoint : Endpoint<Request, Response>
{
    private readonly GameService _gameService;

    public Endpoint(GameService gameService)
    {
        _gameService = gameService;
    }

    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("/game/start");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var gameId = await _gameService.CreateGame(req.PlayerName);
        Response.GameId = gameId;
    }
}
