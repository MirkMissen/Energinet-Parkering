using Case.Domain.Abstractions;
using Case.Domain.Repositories;
using Case.Infrastructure.Repositories;
using Case.Infrastructure.Time;
using Microsoft.Extensions.DependencyInjection;

namespace Case.Infrastructure
{
    public static class Registration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services
                .AddScoped<IWeeklyParkingSpotRepository, InMemoryWeeklyParkingSpotRepository>()
                .AddScoped<IUserRepository, InMemoryUserRepository>()
                .AddSingleton<IClock, Clock>();
            return services;
        }
    }
}
