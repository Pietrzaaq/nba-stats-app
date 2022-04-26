using NbaStats.Application.Abstractions;
using NbaStats.Application.DTO;

namespace NbaStats.Application.Queries
{
    public class GetGame : IQuery<GameDto>
    {
        public int GameId { get; set; }
    }
}