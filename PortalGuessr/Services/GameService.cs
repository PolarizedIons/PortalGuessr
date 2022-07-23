using Microsoft.EntityFrameworkCore;
using PortalGuessr.Database;
using PortalGuessr.Database.Entities;
using PortalGuessr.Features.Game.GetGuess;

namespace PortalGuessr.Services;

public class GameService
{
    private readonly DatabaseContext _db;
    private readonly LeaderboardService _leaderboardService;
    private readonly Random _random;

    public GameService(DatabaseContext db, LeaderboardService leaderboardService)
    {
        _db = db;
        _leaderboardService = leaderboardService;
        _random = new Random();
    }

    public async Task<Guid> CreateGame(string playerName)
    {
        var game = new Game
        {
            PlayerName = playerName,
            State = GameState.InProgress,
        };

        _db.Games.Add(game);
        await _db.SaveChangesAsync();

        return game.Id;
    }

    public async Task<(LocationGuess guess, IEnumerable<GuessChoice> choices)> CreateGuess(Guid gameId)
    {
        var game = await _db.Games.FirstAsync(x => x.Id == gameId);

        var locationCount = await _db.GameLocations.CountAsync();
        var locations = new List<GameLocation>();
        for (var i = 0; i < 4; i++)
        {
            var location = await _db.GameLocations
                .Skip(_random.Next(locationCount))
                .Take(1)
                .FirstAsync();
            locations.Add(location);
        }

        var guess = new LocationGuess
        {
            Game = game,
            State = GuessState.Seen,
            Location = locations.First(),
        };

        _db.Guesses.Add(guess);
        await _db.SaveChangesAsync();

        var choices = locations
            .Select(x => new GuessChoice
            {
                Id = x.Id,
                Name = x.Name,
            })
            .OrderBy(_ => _random.Next());

        return (guess, choices);
    }

    public async Task<LocationGuess> MakeGuess(Guid gameId, Guid guessId, Guid guessedLocationId)
    {
        var guess = await _db.Guesses
            .Include(x => x.Location)
            .Include(x => x.Game)
            .FirstAsync(x => x.Id == guessId);
        var game = guess.Game;
        
        guess.GuessedAt = DateTime.UtcNow;
        
        // skipped
        if (guessId == Guid.Empty)
        {
            guess.State = GuessState.Skipped;
        }
        // not-skipped
        else
        {
            guess.State = guess.Location.Id == guessedLocationId ? GuessState.Correct : GuessState.Incorrect;
            var score = (long)Math.Ceiling(Math.Max(0, 100 - guess.GuessDuration.TotalSeconds) * (guess.GuessDuration.Seconds < 5 ? 1.2 : 1));
            game.Score += score;

            if (game.Guesses.Count >= 10)
            {
                game.State = GameState.Completed;
                await _leaderboardService.CreateEntry(game.PlayerName, game.Score);
            }
        }

        await _db.SaveChangesAsync();
        return guess;
    }

    public async Task<bool> IsValidGame(Guid gameId)
    {
        return await _db.Games.AnyAsync(x => x.Id == gameId);
    }

    public async Task<bool> IsGameInProgress(Guid gameId)
    {
        var game = await _db.Games.FirstOrDefaultAsync(x => x.Id == gameId);
        return game?.State == GameState.InProgress && game.CreatedAt > DateTime.UtcNow.AddHours(-1);
    }

    public async Task<bool> IsValidGuess(Guid guessId)
    {
        return await _db.Guesses.AnyAsync(x => x.Id == guessId);
    }

    public async Task<bool> IsGuessNotAnswered(Guid guessId)
    {
        var guess = await _db.Guesses.FirstOrDefaultAsync(x => x.Id == guessId);
        return guess?.State == GuessState.Seen;
    }

    public async Task<bool> IsValidLocation(Guid locationId)
    {
        return await _db.GameLocations.AnyAsync(x => x.Id == locationId);
    }

    public async Task<List<LeaderboardEntry>> GetLeaderboard()
    {
        return await _db.Leaderboard
            .OrderByDescending(x => x.Score)
            .Take(50)
            .ToListAsync();
    }
}
