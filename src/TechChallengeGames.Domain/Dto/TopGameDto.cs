using TechChallengeGames.Domain.Enums;

namespace TechChallengeGames.Domain.Dto;

public record TopGameDto(Guid Id, string Name, string Description, Category Category, long Purchases);