using Microsoft.EntityFrameworkCore;
using PortalGuessr.Database.Entities.Abstractions;

namespace PortalGuessr.Database.Entities;

public class GameLocation : DbEntity
{
    public string Name { get; set; } = null!;
    public Guid PictureId { get; set; }
}
