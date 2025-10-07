using TechChallengeGames.Domain.Dto;
using TechChallengeGames.Domain.Enums;

namespace TechChallengeGames.Domain.Models;

public sealed class Game(string name, string description, string imageUrl, Category category, DateTime releaseDate, double price) : Entity
{
    public string Name { get; private set; } = name;
    public string Description { get; private set; } = description;
    public string ImageUrl { get; private set; } = imageUrl;
    public Category Category { get; private set; } = category;
    public DateTime ReleaseDate { get; private set; } = releaseDate;
    public double Price { get; private set; } = price;

    public void Update(string name, string description, string imageUrl, Category category, DateTime releaseDate, double price)
    {
        Name = name;
        Description = description;
        ImageUrl = imageUrl;
        Category = category;
        ReleaseDate = releaseDate;
        Price = price;
    }

    public GameDto MapToDto()
        => new(Id, Active, CreatedIn, UpdatedIn, Name, Description, ImageUrl, Category, ReleaseDate, Price);
}