namespace TechChallengeGames.Elasticsearch;

public class ElasticSettings : IElasticSettings
{
    public string ApiKey { get; set; } = string.Empty;
    public string CloudId { get; set; } = string.Empty;
}