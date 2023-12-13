using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Case.Application
{
    public static class Registration
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = typeof(Registration).Assembly;
            services.AddMediatR(assembly);
            return services;
        }
    }
}
