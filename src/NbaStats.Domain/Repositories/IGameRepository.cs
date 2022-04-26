using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NbaStats.Domain.Entities;

namespace NbaStats.Domain.Repositories
{
    public interface IGameRepository
    {
        Task<Game> GetAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<IReadOnlyList<Game>> BrowseAsync();
        Task<IReadOnlyList<Game>> BrowseAsync(DateTime date);
        Task AddAsync(Game game);
        Task UpdateAsync(Game game);
    }
}