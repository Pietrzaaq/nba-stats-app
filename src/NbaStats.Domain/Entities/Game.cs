using System;
using System.Collections.Generic;

namespace NbaStats.Domain.Entities
{
    public partial class Game
    {
        public Game()
        {
            Quarters = new HashSet<Quarter>();
        }

        public int GameId { get; set; }
        public bool IsApiId { get; set; }
        public int? Season { get; set; }
        public int? SeasonType { get; set; }
        public string Status { get; set; }
        public DateTime? Day { get; set; }
        public DateTime? DateTime { get; set; }
        public string AwayTeam { get; set; }
        public string HomeTeam { get; set; }
        public int AwayTeamId { get; set; }
        public int HomeTeamId { get; set; }
        public int? StadiumId { get; set; }
        public string? Channel { get; set; }
        public int? AwayTeamScore { get; set; }
        public int? HomeTeamScore { get; set; }
        public DateTime? Updated { get; set; }
        public double? PointSpread { get; set; }
        public int? AwayTeamMoneyLine { get; set; }
        public int? HomeTeamMoneyLine { get; set; }
        public int? GlobalGameId { get; set; }
        public int? GlobalAwayTeamId { get; set; }
        public int? GlobalHomeTeamId { get; set; }
        public int? PointSpreadAwayTeamMoneyLine { get; set; }
        public int? PointSpreadHomeTeamMoneyLine { get; set; }
        public string? LastPlay { get; set; }
        public bool? IsClosed { get; set; }
        public DateTime? GameEndDateTime { get; set; }
        public int? HomeRotationNumber { get; set; }
        public int? AwayRotationNumber { get; set; }
        public bool? NeutralVenue { get; set; }
        public DateTime? DateTimeUtc { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? RefreshDate { get; set; }

        public virtual Team AwayTeamNavigation { get; set; }
        public virtual Team HomeTeamNavigation { get; set; }
        public virtual ICollection<Quarter> Quarters { get; set; }
    }
}