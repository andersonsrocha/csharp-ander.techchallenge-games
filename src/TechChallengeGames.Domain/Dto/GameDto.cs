using TechChallengeGames.Domain.Enums;

namespace TechChallengeGames.Domain.Dto;

public record GameDto(Guid Id, bool Active, DateTime CreatedIn, DateTime? UpdatedIn, string Name, string Description, string ImageUrl, Category Category, DateTime ReleaseDate, double Price);