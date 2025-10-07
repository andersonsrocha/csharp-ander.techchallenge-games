using TechChallengeGames.Domain.Enums;

namespace TechChallengeGames.Domain.Models;

public class GameLog(Guid id, string name, string description, Category category)
{
    public Guid Id { get; private set; } = id;
    public string Name { get; private set; } = name;
    public string Description { get; private set; } = description;
    public Category Category { get; private set; } = category;
}