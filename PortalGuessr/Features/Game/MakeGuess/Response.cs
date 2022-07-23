namespace PortalGuessr.Features.Game.MakeGuess;

public class Response
{
    public bool GameComplete { get; set; }
    public bool Correct { get; set; }
    public string LocationName { get; set; } = null!;
    public long RunningScore { get; set; }
}
