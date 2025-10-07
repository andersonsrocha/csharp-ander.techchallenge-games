using TechChallengeGames.Domain.Interfaces;
using TechChallengeGames.Domain.Models;

namespace TechChallengeGames.Data.Repositories;

public class GameRepository(TechChallengeGamesContext context) : Repository<Game>(context), IGameRepository;