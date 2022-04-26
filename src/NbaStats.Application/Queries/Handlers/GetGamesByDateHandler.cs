using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NbaStats.Application.Abstractions;
using NbaStats.Application.DTO;
using NbaStats.Domain.Repositories;

namespace NbaStats.Application.Queries.Handlers
{
    public class GetGamesByDateHandler : IQueryHandler<GetGamesByDate, List<GameDto>>
    {
        private readonly IGameRepository _gameRepository;

        public GetGamesByDateHandler(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task<List<GameDto>> HandleAsync(GetGamesByDate query)
        {
            var games = await _gameRepository.BrowseAsync(query.Date);

            return games.Select(x => x.AsDto()).OrderBy(x => x.DateTimeUtc).ToList();
        }
    }
}