using FastEndpoints;
using PortalGuessr.Database.Entities;
using PortalGuessr.Services;

namespace PortalGuessr.Features.Game.MakeGuess;

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
        Routes("/game/{GameId}/guess");
        AllowAnonymous();
        ScopedValidator();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var result = await _gameService.MakeGuess(req.GameId, req.GuessId, req.GuessedLocationId);

        Response.GameComplete = result.Game.State == GameState.Completed;
        Response.Correct = result.State == GuessState.Correct;
        Response.LocationName = result.Location.Name;
        Response.RunningScore = result.Game.Score;
    }
}
