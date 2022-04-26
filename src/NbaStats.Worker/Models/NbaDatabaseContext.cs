using Microsoft.EntityFrameworkCore;
using NbaStats.Domain.Entities;

#nullable disable

namespace NbaStats.Worker.Models
{
public partial class NbaDatabaseContext : DbContext
    {
        public NbaDatabaseContext()
        {
        }

        public NbaDatabaseContext(DbContextOptions<NbaDatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<Quarter> Quarters { get; set; }
        public virtual DbSet<Team> Teams { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=localhost; Initial Catalog=NbaDatabase; TrustServerCertificate=True; User Id=user; Password=12345");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Polish_CI_AS");

            modelBuilder.Entity<Game>(entity =>
            {
                entity.ToTable("Games", "StatsManagement");

                entity.Property(e => e.GameId).ValueGeneratedNever();

                entity.Property(e => e.AwayTeam)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Channel)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DateTime).HasColumnType("datetime");

                entity.Property(e => e.DateTimeUtc)
                    .HasColumnType("datetime")
                    .HasColumnName("DateTimeUTC");
                
                entity.Property(e => e.GameEndDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("GameEndDateTime");

                entity.Property(e => e.Day).HasColumnType("date");

                entity.Property(e => e.HomeTeam)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastPlay)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RefreshDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Status)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Updated).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.AwayTeamNavigation)
                    .WithMany(p => p.GameAwayTeamNavigations)
                    .HasForeignKey(d => d.AwayTeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Games_AwayTeamId");

                entity.HasOne(d => d.HomeTeamNavigation)
                    .WithMany(p => p.GameHomeTeamNavigations)
                    .HasForeignKey(d => d.HomeTeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Games_HomeTeamId");
            });

            modelBuilder.Entity<Player>(entity =>
            {
                entity.ToTable("Players", "StatsManagement");

                entity.Property(e => e.PlayerId).ValueGeneratedNever();

                entity.Property(e => e.BirthCity)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BirthCountry)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BirthDate).HasColumnType("datetime");

                entity.Property(e => e.BirthState)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.College)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DepthChartPosition)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.HighSchool)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.InjuryBodyPart)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.InjuryNotes)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.InjuryStartDate).HasColumnType("date");

                entity.Property(e => e.InjuryStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PhotoUrl).IsUnicode(false);

                entity.Property(e => e.Position)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PositionCategory)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RefreshDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.SportsDataId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Team)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UsaTodayHeadshotNoBackgroundUpdated).HasColumnType("datetime");

                entity.Property(e => e.UsaTodayHeadshotNoBackgroundUrl).IsUnicode(false);

                entity.Property(e => e.UsaTodayHeadshotUpdated).HasColumnType("datetime");

                entity.Property(e => e.UsaTodayHeadshotUrl).IsUnicode(false);

                entity.HasOne(d => d.TeamNavigation)
                    .WithMany(p => p.Players)
                    .HasForeignKey(d => d.TeamId)
                    .HasConstraintName("FK_Players_TeamId");
            });

            modelBuilder.Entity<Quarter>(entity =>
            {
                entity.ToTable("Quarters", "StatsManagement");

                entity.Property(e => e.QuarterId).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.RefreshDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.Quarters)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Quarters_GameId");
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.ToTable("Teams", "StatsManagement");

                entity.Property(e => e.TeamId).ValueGeneratedNever();

                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Conference)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Division)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Key)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PrimaryColor)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.QuaternaryColor)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RefreshDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.SecondaryColor)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TertiaryColor)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.WikipediaLogoUrl).IsUnicode(false);

                entity.Property(e => e.WikipediaWordMarkUrl).IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
