using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NbaStats.Domain.Entities;

namespace NbaStats.Infrastructure.DAL.Configurations
{
    public class PlayerConfiguration : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            builder.ToTable("Players", "StatsManagement");

            builder.Property(e => e.PlayerId).ValueGeneratedNever();

            builder.Property(e => e.BirthCity)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.BirthCountry)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.BirthDate).HasColumnType("datetime");

            builder.Property(e => e.BirthState)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.College)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.DepthChartPosition)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.HighSchool)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.InjuryBodyPart)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.InjuryNotes)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.InjuryStartDate).HasColumnType("date");

            builder.Property(e => e.InjuryStatus)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.PhotoUrl).IsUnicode(false);

            builder.Property(e => e.Position)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.PositionCategory)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.RefreshDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            builder.Property(e => e.SportsDataId)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.Team)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.UsaTodayHeadshotNoBackgroundUpdated).HasColumnType("datetime");

            builder.Property(e => e.UsaTodayHeadshotNoBackgroundUrl).IsUnicode(false);

            builder.Property(e => e.UsaTodayHeadshotUpdated).HasColumnType("datetime");

            builder.Property(e => e.UsaTodayHeadshotUrl).IsUnicode(false);

            builder.HasOne(d => d.TeamNavigation)
                .WithMany(p => p.Players)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("FK_Players_TeamId");
        }
    }
}