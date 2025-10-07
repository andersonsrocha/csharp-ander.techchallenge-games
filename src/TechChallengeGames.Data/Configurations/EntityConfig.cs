using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechChallengeGames.Domain.Models;

namespace TechChallengeGames.Data.Configurations;

public abstract class EntityConfig<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : Entity
{
    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Active)
            .HasColumnType("boolean")
            .IsRequired();

        builder.Property(x => x.CreatedIn)
            .HasDefaultValueSql("now()")
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(x => x.UpdatedIn)
            .HasDefaultValue(null);

        builder.Property(x => x.DeletedIn)
            .IsRequired(false)
            .HasDefaultValue(null);

        Map(builder);
    }

    protected abstract void Map(EntityTypeBuilder<TEntity> builder);
}