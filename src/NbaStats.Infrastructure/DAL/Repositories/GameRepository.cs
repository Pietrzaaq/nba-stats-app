using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NbaStats.Domain.Entities;
using NbaStats.Domain.Repositories;

namespace NbaStats.Infrastructure.DAL.Repositories
{
    internal sealed class GameRepository : IGameRepository
    {
        private readonly NbaDatabaseContext _context;
        private readonly DbSet<Game> _games;

        public GameRepository(NbaDatabaseContext context)
        {
            _context = context;
            _games = context.Games;
        }

        public Task<Game?> GetAsync(int id)
        {
            return _games
                .SingleOrDefaultAsync(x => x.GameId == id);
        }

        public Task<bool> ExistsAsync(int id)
        {
            return _games.AnyAsync(x => x.GameId == id);
        }

        public async Task<IReadOnlyList<Game>> BrowseAsync()
        {
            return await _games.ToListAsync();
        }

        public async Task<IReadOnlyList<Game>> BrowseAsync(DateTime date)
        {
            return await _games.Where(x => x.Day.Value.Date == date.Date).ToListAsync();
        }

        public async Task AddAsync(Game game)
        {
            await _games.AddAsync(game);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Game game)
        {
            _games.Update(game);
            await _context.SaveChangesAsync();
        }
    }
}