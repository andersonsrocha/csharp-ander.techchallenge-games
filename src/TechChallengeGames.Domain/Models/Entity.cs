namespace TechChallengeGames.Domain.Models;

public abstract class Entity
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public bool Active { get; private set; } = true;
    public DateTime CreatedIn { get; init; } = DateTime.Now.ToUniversalTime();
    public DateTime? UpdatedIn { get; private set; }
    public DateTime? DeletedIn { get; private set; }
    
    public void MarkAsDeleted()
    {
        Active = false;
        DeletedIn = DateTime.Now.ToUniversalTime();
    }
}