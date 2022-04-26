using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using NbaStats.Application.Abstractions;
using NbaStats.Application.DTO;
using NbaStats.Application.Queries;
using NbaStats.Application.Queries.Handlers;

namespace NbaStats.Application
{
    public static class Extensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IQueryHandler<GetGame, GameDto>, GetGameHandler>();
            services.AddScoped<IQueryHandler<GetGamesByDate, List<GameDto>>, GetGamesByDateHandler>();
            services.AddScoped<IQueryHandler<GetTeam, TeamDto>, GetTeamHandler>();

            return services;
        }
    }
}