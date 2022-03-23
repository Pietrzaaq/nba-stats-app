using System.Collections.Generic;
using System.Threading.Tasks;
using NbaStats.Domain.Entities;

namespace NbaStats.Domain.Repositories
{
    public interface ITeamRepository
    {
        Task AddAsync(Team team);
        Task<IReadOnlyList<Team>> BrowseAsync();
    }
}