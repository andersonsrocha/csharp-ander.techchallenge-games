using TechChallengeGames.Domain.Enums;

namespace TechChallengeGames.Domain.Models;

public sealed class Game(string name, string description, string imageUrl, Category category, DateTime releaseDate) : Entity
{
    public string Name { get; private set; } = name;
    public string Description { get; private set; } = description;
    public string ImageUrl { get; private set; } = imageUrl;
    public Category Category { get; private set; } = category;
    public DateTime ReleaseDate { get; private set; } = releaseDate;

    public void Update(string name, string description, string imageUrl, Category category, DateTime releaseDate)
    {
        Name = name;
        Description = description;
        ImageUrl = imageUrl;
        Category = category;
        ReleaseDate = releaseDate;
    }
}