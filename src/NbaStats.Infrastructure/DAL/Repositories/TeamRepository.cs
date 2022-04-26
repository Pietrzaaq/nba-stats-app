using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NbaStats.Domain.Entities;
using NbaStats.Domain.Repositories;

namespace NbaStats.Infrastructure.DAL.Repositories
{
    internal sealed class TeamRepository : ITeamRepository
    {
        private readonly NbaDatabaseContext _context;
        private readonly DbSet<Team> _teams;

        public TeamRepository(NbaDatabaseContext context)
        {
            _context = context;
            _teams = _context.Teams;
        }

        public Task<Team> GetAsync(int id)
        {
            return _teams
                .SingleOrDefaultAsync(x => x.TeamId == id)!;
        }

        public Task<bool> ExistsAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<IReadOnlyList<Team>> BrowseAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task AddAsync(Team team)
        {
            throw new System.NotImplementedException();
        }
    }
}