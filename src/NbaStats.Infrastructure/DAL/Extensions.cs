using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NbaStats.Domain.Entities;
using NbaStats.Domain.Repositories;
using NbaStats.Infrastructure.DAL.Repositories;

namespace NbaStats.Infrastructure.DAL
{
    internal static class Extensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration[$"database:{nameof(DatabaseOptions.ConnectionString)}"];
            services.AddDbContext<NbaDatabaseContext>(x => x.UseSqlServer(connectionString));
            
            services.AddScoped<IGameRepository, GameRepository>();
            services.AddScoped<ITeamRepository, TeamRepository>();

            return services;
        }
    }
}