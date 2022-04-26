using Microsoft.EntityFrameworkCore;
using NbaStats.Domain.Entities;

#nullable disable

namespace NbaStats.Infrastructure.DAL
{
    internal sealed class NbaDatabaseContext : DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Team> Teams { get; set; }
        
        public NbaDatabaseContext(DbContextOptions<NbaDatabaseContext> options)
            : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
