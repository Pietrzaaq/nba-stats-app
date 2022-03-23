using System.Collections.Generic;
using System.Threading.Tasks;
using NbaStats.Domain.Entities;

namespace NbaStats.Domain.Repositories
{
    public interface IGameRepository
    {
        Task AddAsync(Game game);
        Task<IReadOnlyList<Game>> BrowseAsync();
    }
}