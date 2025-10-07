using Microsoft.EntityFrameworkCore;
using TechChallengeGames.Domain.Models;

namespace TechChallengeGames.Data;

public class TechChallengeGamesContext(DbContextOptions<TechChallengeGamesContext> options) : DbContext(options)
{
    public DbSet<Game> Games { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(TechChallengeGamesContext).Assembly);
    }
}