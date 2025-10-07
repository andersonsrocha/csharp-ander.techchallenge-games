using TechChallengeGames.Domain.Models;

namespace TechChallengeGames.Domain.Interfaces;

public interface IRepository<TEntity> where TEntity : Entity
{
    TEntity? Find(Guid id);
    IEnumerable<TEntity> Find(IEnumerable<Guid> ids);
    IEnumerable<TEntity> Find();
    void Add(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
}