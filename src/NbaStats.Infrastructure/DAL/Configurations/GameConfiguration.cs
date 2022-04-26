using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NbaStats.Domain.Entities;

namespace NbaStats.Infrastructure.DAL.Configurations
{
    public class GameConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        { 
            builder.ToTable("Games", "StatsManagement");

            builder.Property(e => e.GameId).ValueGeneratedNever();

            builder.Property(e => e.AwayTeam)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.Channel)
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(e => e.DateTime).HasColumnType("datetime");

            builder.Property(e => e.DateTimeUtc)
                .HasColumnType("datetime")
                .HasColumnName("DateTimeUTC");
            
            builder.Property(e => e.GameEndDateTime)
                .HasColumnType("datetime")
                .HasColumnName("GameEndDateTime");

            builder.Property(e => e.Day).HasColumnType("date");

            builder.Property(e => e.HomeTeam)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.LastPlay)
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(e => e.RefreshDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            builder.Property(e => e.Status)
                .HasMaxLength(30)
                .IsUnicode(false);

            builder.Property(e => e.Updated).HasColumnType("datetime");

            builder.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.HasOne(d => d.AwayTeamNavigation)
                .WithMany(p => p.GameAwayTeamNavigations)
                .HasForeignKey(d => d.AwayTeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Games_AwayTeamId");

            builder.HasOne(d => d.HomeTeamNavigation)
                .WithMany(p => p.GameHomeTeamNavigations)
                .HasForeignKey(d => d.HomeTeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Games_HomeTeamId");
        }
    }
}