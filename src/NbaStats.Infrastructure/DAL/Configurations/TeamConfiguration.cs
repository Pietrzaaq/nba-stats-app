using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NbaStats.Domain.Entities;

namespace NbaStats.Infrastructure.DAL.Configurations
{
    public class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.ToTable("Teams", "StatsManagement");

            builder.Property(e => e.TeamId).ValueGeneratedNever();

            builder.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.Conference)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.Division)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.Key)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.PrimaryColor)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.QuaternaryColor)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.RefreshDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            builder.Property(e => e.SecondaryColor)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.TertiaryColor)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.WikipediaLogoUrl).IsUnicode(false);

            builder.Property(e => e.WikipediaWordMarkUrl).IsUnicode(false);
        }
    }
}