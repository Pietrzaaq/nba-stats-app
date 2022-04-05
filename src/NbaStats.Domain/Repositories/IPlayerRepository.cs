using System.Collections.Generic;
using System.Threading.Tasks;
using NbaStats.Domain.Entities;

namespace NbaStats.Domain.Repositories
{
    public interface IPlayerRepository
    {
        Task<Player> GetAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task AddAsync(Player player);
        Task<IReadOnlyList<Player>> BrowseAsync();
    }
}