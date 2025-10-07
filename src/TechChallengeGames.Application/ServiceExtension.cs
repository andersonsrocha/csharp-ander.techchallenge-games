using Microsoft.Extensions.DependencyInjection;

namespace TechChallengeGames.Application;

public static class ServiceExtension
{
    public static void AddServices(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblies(typeof(IService).Assembly, typeof(Service).Assembly)
            .AddClasses(c => c.AssignableTo<IService>())
            .AsImplementedInterfaces()
            .WithScopedLifetime());
    }
}