using Elastic.Clients.Elasticsearch;

namespace TechChallengeGames.Elasticsearch;

public interface IElasticClient<T>
{
    Task<SearchResponse<TU>> Search<TU>(Action<SearchRequestDescriptor<TU>> action);
    Task<IReadOnlyCollection<T>> Find(int page, int size, IndexName index);
    Task<bool> AddOrUpdate(T item, IndexName index);
}