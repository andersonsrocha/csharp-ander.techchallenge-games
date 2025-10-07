using TechChallengeGames.Domain.Enums;
using TechChallengeGames.Domain.Models;

namespace TechChallengeGames.Application.Games.Commands;

public class CreateGameRequest
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string ImageUrl { get; init; } = string.Empty;
    public Category Category { get; init; }
    public DateTime ReleaseDate { get; init; } = DateTime.Now;

    public static implicit operator Game(CreateGameRequest request)
    {
        return new Game(request.Name, request.Description, request.ImageUrl, request.Category, request.ReleaseDate);
    }
}