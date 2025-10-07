using TechChallengeGames.Domain.Enums;

namespace TechChallengeGames.Application.Games.Commands;

public class UpdateGameRequest
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string ImageUrl { get; init; } = string.Empty;
    public Category Category { get; init; }
    public DateTime ReleaseDate { get; init; } = DateTime.Now;
}