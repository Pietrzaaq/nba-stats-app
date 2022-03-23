﻿using System;

namespace NbaStats.Domain.Entities
{
    public class Game
    {
        public int GameId { get; set; }
        public int ApiGameId { get; set; }
        public int? Season { get; set; }
        public int? SeasonType { get; set; }
        public string Status { get; set; }
        public DateTime? Day { get; set; }
        public DateTime? DateTime { get; set; }
        public string AwayTeam { get; set; }
        public string HomeTeam { get; set; }
        public int AwayTeamId { get; set; }
        public int HomeTeamId { get; set; }
        public int ApiAwayTeamId { get; set; }
        public int ApiHomeTeamId { get; set; }
        public int? StadiumId { get; set; }
        public string Channel { get; set; }
        public int? AwayTeamScore { get; set; }
        public int? HomeTeamScore { get; set; }
        public DateTime? Updated { get; set; }
        public string Quarter { get; set; }
        public double? PointSpread { get; set; }
        public int? AwayTeamMoneyLine { get; set; }
        public int? HomeTeamMoneyLine { get; set; }
        public int? GlobalGameId { get; set; }
        public int? GlobalAwayTeamId { get; set; }
        public int? GlobalHomeTeamId { get; set; }
        public int? PointSpreadAwayTeamMoneyLine { get; set; }
        public int? PointSpreadHomeTeamMoneyLine { get; set; }
        public string LastPlay { get; set; }
        public bool? IsClosed { get; set; }
        public int? HomeRotationNumber { get; set; }
        public int? AwayRotationNumber { get; set; }
        public bool? NeutralVenue { get; set; }
        public DateTime? DateTimeUtc { get; set; }

        public virtual Team AwayTeamNavigation { get; set; }
        public virtual Team HomeTeamNavigation { get; set; }
    }
}