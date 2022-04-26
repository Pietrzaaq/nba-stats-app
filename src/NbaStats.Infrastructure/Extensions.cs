using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NbaStats.Application.Abstractions;
using NbaStats.Infrastructure.Commands;
using NbaStats.Infrastructure.DAL;
using NbaStats.Infrastructure.Queries;

namespace NbaStats.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<IDispatcher, InMemoryDispatcher>();
            services.AddSingleton<IQueryDispatcher, InMemoryQueryDispatcher>();
            services.AddSingleton<ICommandDispatcher, InMemoryCommandDispatcher>();

            services.AddDatabase(configuration);

            return services;
        }
    }
}