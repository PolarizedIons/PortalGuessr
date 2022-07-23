using Microsoft.AspNetCore.Mvc;

namespace PortalGuessr.Features.Game.GetLocationPicture;

public class Request
{
    [FromRoute]
    public Guid LocationId { get; set; }
}
