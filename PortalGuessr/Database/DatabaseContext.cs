using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PortalGuessr.Database.Entities;
using PortalGuessr.Database.Entities.Abstractions;

namespace PortalGuessr.Database;

public sealed class DatabaseContext : DbContext
{
    public DbSet<Game> Games { get; set; } = null!;
    public DbSet<LocationGuess> Guesses { get; set; } = null!;
    public DbSet<GameLocation> GameLocations { get; set; } = null!;
    public DbSet<LeaderboardEntry> Leaderboard { get; set; } = null!;

    public DatabaseContext(DbContextOptions options) : base(options)
    {
        ChangeTracker.Tracked += OnEntityTracked;
        ChangeTracker.StateChanged += OnEntityStateChanged;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (typeof(DbEntity).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "x");
                var property = Expression.Equal(Expression.Property(parameter, nameof(DbEntity.DeletedAt)),
                    Expression.Constant(null));
                var delegateType = typeof(Func<,>).MakeGenericType(entityType.ClrType, typeof(bool));

                var filterExpression = Expression.Lambda(delegateType, property, parameter);
                entityType.SetQueryFilter(filterExpression);
            }
        }
    }

    private void OnEntityTracked(object? sender, EntityTrackedEventArgs e)
    {
        if (e.Entry.State == EntityState.Added && e.Entry.Entity is DbEntity entity)
        {
            entity.Id = Guid.NewGuid();
            entity.CreatedAt = DateTime.UtcNow;
            entity.ModifiedAt = entity.CreatedAt;
        }
    }

    private void OnEntityStateChanged(object? sender, EntityStateChangedEventArgs e)
    {
        if (e.Entry.Entity is not DbEntity entity)
        {
            return;
        }

        switch (e)
        {
            case { NewState: EntityState.Modified }:
                entity.ModifiedAt = DateTime.UtcNow;
                break;
            case { NewState: EntityState.Deleted }:
                entity.DeletedAt = DateTime.UtcNow;
                e.Entry.State = EntityState.Modified;
                break;
        }
    }
}
