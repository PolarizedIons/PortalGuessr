namespace PortalGuessr.Features.Game.GetGuess;

public class Response
{
    public Guid GuessId { get; set; }
    public Guid PictureId { get; set; }
    public IEnumerable<GuessChoice> Choice { get; set; } = null!;
}

public class GuessChoice
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
}
