using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NbaStats.Domain.Entities;

namespace NbaStats.Application.DTO
{
    public class GameDto
    {
        [Required]
        public int GameId { get; set; }
        public bool IsApiId { get; set; }
        public int? Season { get; set; }
        public string Status { get; set; }
        public DateTime? DateTime { get; set; }
        public string AwayTeam { get; set; }
        public string HomeTeam { get; set; }
        public int? AwayTeamId { get; set; }
        public int? HomeTeamId { get; set; }
        public int? AwayTeamScore { get; set; }
        public int? HomeTeamScore { get; set; }
        public DateTime? Updated { get; set; }
        public List<Quarter> Quarter { get; set; }
        public bool? IsClosed { get; set; }
        public DateTime? DateTimeUtc { get; set; }
    }
}