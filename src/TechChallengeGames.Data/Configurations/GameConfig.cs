using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechChallengeGames.Domain.Models;

namespace TechChallengeGames.Data.Configurations;

public sealed class GameConfig : EntityConfig<Game>
{
    protected override void Map(EntityTypeBuilder<Game> builder)
    {
        builder.Property(x => x.Name)
            .HasMaxLength(150)
            .IsRequired();
        
        builder.Property(x => x.Description)
            .HasMaxLength(200)
            .IsRequired();
        
        builder.Property(x => x.ImageUrl)
            .HasMaxLength(200)
            .IsRequired(false);

        builder.Property(x => x.Category)
            .IsRequired();

        builder.Property(x => x.ReleaseDate)
            .HasColumnType("DATE")
            .IsRequired();
        
        builder.Property(x => x.Price)
            .HasColumnType("DECIMAL(10,2)")
            .IsRequired();
    }
}