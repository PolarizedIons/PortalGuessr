

using System.Text.RegularExpressions;
using FastEndpoints;
using FluentValidation;

namespace PortalGuessr.Features.Game.Start;

public class Request
{
    public string PlayerName { get; set; } = "HoopyFan98";
}

public class Validator : Validator<Request>
{
    public Validator()
    {
        RuleFor(x => x.PlayerName)
            .Matches(new Regex("\\w*"))
            .MaximumLength(32);
    }
}
