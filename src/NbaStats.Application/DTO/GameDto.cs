using System;
using System.ComponentModel.DataAnnotations;

namespace NbaStats.Application.DTO
{
    public class GameDto
    {
        [Required]
        public int GameId { get; set; }
        public int ApiGameId { get; set; }
        public int Season { get; set; }
        public string Status { get; set; }
        public DateTime DateTime { get; set; }
        public string AwayTeam { get; set; }
        public string HomeTeam { get; set; }
        [Required]
        public int AwayTeamId { get; set; }
        [Required]
        public int HomeTeamId { get; set; }
        public int ApiAwayTeamId { get; set; }
        public int ApiHomeTeamId { get; set; }
        public int AwayTeamScore { get; set; }
        public int HomeTeamScore { get; set; }
        public DateTime Updated { get; set; }
        public string Quarter { get; set; }
        public bool IsClosed { get; set; }
        public DateTime? DateTimeUtc { get; set; }
    }
}