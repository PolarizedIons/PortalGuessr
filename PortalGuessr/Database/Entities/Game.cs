using PortalGuessr.Database.Entities.Abstractions;

namespace PortalGuessr.Database.Entities;

public class Game : DbEntity
{
    public string PlayerName { get; set; } = null!;
    public GameState State { get; set; }
    
    public ICollection<LocationGuess> Guesses { get; set; } = null!;
    
    public long Score { get; set; }
}

public enum GameState
{
    InProgress,
    Completed,
}
