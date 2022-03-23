using System.Collections.Generic;
using System.Threading.Tasks;
using NbaStats.Domain.Entities;

namespace NbaStats.Domain.Repositories
{
    public interface IPlayerRepository
    {
        Task AddAsync(Player player);
        Task<IReadOnlyList<Player>> BrowseAsync();
    }
}