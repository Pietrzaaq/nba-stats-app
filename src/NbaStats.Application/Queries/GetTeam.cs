using NbaStats.Application.Abstractions;
using NbaStats.Application.DTO;

namespace NbaStats.Application.Queries
{
    public class GetTeam : IQuery<TeamDto>
    {
        public int TeamId { get; set; }
    }
}