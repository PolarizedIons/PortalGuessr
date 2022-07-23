using FastEndpoints;
using PortalGuessr.Services;

namespace PortalGuessr.Features.Game.GetGuess;

public class Endpoint : Endpoint<Request, Response>
{
    private readonly GameService _gameService;

    public Endpoint(GameService gameService)
    {
        _gameService = gameService;
    }

    public override void Configure()
    {
        Verbs(Http.GET);
        Routes("/game/{GameId}/guess");
        AllowAnonymous();
        ScopedValidator();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var (guess, choices) = await _gameService.CreateGuess(req.GameId);

        Response.GuessId = guess.Id;
        Response.PictureId = guess.Location.PictureId;
        Response.Choice = choices;
    }
}