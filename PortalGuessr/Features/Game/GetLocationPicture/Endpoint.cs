using FastEndpoints;

namespace PortalGuessr.Features.Game.GetLocationPicture;

public class Endpoint : Endpoint<Request>
{
    public override void Configure()
    {
        Verbs(Http.GET);
        Routes("/picture/{LocationId}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        await SendFileAsync(new FileInfo($"./pictures/{req.LocationId}"), cancellation: ct);
    }
}
