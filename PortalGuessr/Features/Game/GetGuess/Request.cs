using System.Data;
using FastEndpoints;
using FluentValidation;
using FluentValidation.Validators;
using Microsoft.AspNetCore.Mvc;
using PortalGuessr.Services;

namespace PortalGuessr.Features.Game.GetGuess;

public class Request
{
    [FromRoute]
    public Guid GameId { get; set; }
}

public class Validator : Validator<Request>
{
    public Validator(GameService gameService)
    {
        RuleFor(x => x.GameId)
            .SetAsyncValidator(new AsyncPredicateValidator<Request, Guid>((_, id, _, _) => gameService.IsValidGame(id)))
            .WithMessage("Invalid game id");

        RuleFor(x => x.GameId)
            .SetAsyncValidator(new AsyncPredicateValidator<Request, Guid>((_, id, _, _) => gameService.IsGameInProgress(id)))
            .WithMessage("Game not in progress");
    }
}
