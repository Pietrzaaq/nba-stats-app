using System.Threading.Tasks;
using NbaStats.Application.Abstractions;
using NbaStats.Application.DTO;
using NbaStats.Domain.Repositories;

namespace NbaStats.Application.Queries.Handlers
{
    public class GetTeamHandler : IQueryHandler<GetTeam,TeamDto>
    {
        private readonly ITeamRepository _teamRepository;

        public GetTeamHandler(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public async Task<TeamDto> HandleAsync(GetTeam query)
        {   
            var team = await _teamRepository.GetAsync(query.TeamId);
            return team.AsDto();
        }
    }
}