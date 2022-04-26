using System.Threading.Tasks;
using NbaStats.Application.Abstractions;
using NbaStats.Application.DTO;
using NbaStats.Domain.Repositories;

namespace NbaStats.Application.Queries.Handlers
{
    public class GetGameHandler : IQueryHandler<GetGame, GameDto>
    {
        private readonly IGameRepository _gameRepository;

        public GetGameHandler(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task<GameDto?> HandleAsync(GetGame query)
        {
            var game = await _gameRepository.GetAsync(query.GameId);

            return game?.AsDto();
        }
    }
}