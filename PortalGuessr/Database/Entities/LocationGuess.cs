using PortalGuessr.Database.Entities.Abstractions;

namespace PortalGuessr.Database.Entities;

public class LocationGuess : DbEntity
{
    public Game Game {get; set; } = null!;

    public GuessState State { get; set; }
    public GameLocation Location { get; set; } = null!;
    public DateTime? GuessedAt { get; set; }

    public TimeSpan GuessDuration => GuessedAt.HasValue ? GuessedAt.Value - CreatedAt : TimeSpan.Zero;
}

public enum GuessState
{
    Seen,
    Skipped,
    Correct,
    Incorrect,
}
