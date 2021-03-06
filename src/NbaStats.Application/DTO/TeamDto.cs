using System;
using System.Collections.Generic;
using NbaStats.Domain.Entities;

namespace NbaStats.Application.DTO
{
    public class TeamDto
    {
        public int TeamId { get; set; }
        public bool? IsApiId { get; set; }
        public string? Key { get; set; }
        public bool? Active { get; set; }
        public string City { get; set; }
        public string Name { get; set; }
        public int? LeagueId { get; set; }
        public int? StadiumId { get; set; }
        public string? Conference { get; set; }
        public string? Division { get; set; }
        public string? PrimaryColor { get; set; }
        public string? SecondaryColor { get; set; }
        public string? TertiaryColor { get; set; }
        public string? QuaternaryColor { get; set; }
        public string? WikipediaLogoUrl { get; set; }
        public string? WikipediaWordMarkUrl { get; set; }
        public int? GlobalTeamId { get; set; }
        public int? NbaDotComTeamId { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? RefreshDate { get; set; }

        public virtual List<Player> Players { get; set; }
    }
}