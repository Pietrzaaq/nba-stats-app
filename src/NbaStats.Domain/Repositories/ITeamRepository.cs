using System.Collections.Generic;
using System.Threading.Tasks;
using NbaStats.Domain.Entities;

namespace NbaStats.Domain.Repositories
{
    public interface ITeamRepository
    {
        Task<Team> GetAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<IReadOnlyList<Team>> BrowseAsync();
        Task AddAsync(Team team);
    }
}