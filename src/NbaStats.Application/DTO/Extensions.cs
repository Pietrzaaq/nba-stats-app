using System.Linq;
using NbaStats.Domain.Entities;

namespace NbaStats.Application.DTO
{
    public static class Extensions
    {
        public static GameDto AsDto(this Game game)
        => new()
        {
            GameId = game.GameId,
            IsApiId = game.IsApiId,
            Season = game.Season,
            Status = game.Status,
            DateTime = game.DateTime,
            AwayTeam = game.AwayTeam,
            HomeTeam = game.HomeTeam,
            AwayTeamId = game.AwayTeamId,
            HomeTeamId = game.HomeTeamId,
            AwayTeamScore = game.AwayTeamScore,
            HomeTeamScore = game.HomeTeamScore,
            Updated = game.Updated,
            Quarter = game.Quarters.ToList(),
            IsClosed = game.IsClosed,
            DateTimeUtc = game.DateTimeUtc
        };

        public static TeamDto AsDto(this Team team)
        => new()
        {
            TeamId =team.TeamId,
            IsApiId = team.IsApiId,
            Key = team.Key,
            Active = team.Active,
            City = team.City,
            Name = team.Name,
            LeagueId = team.LeagueId,
            StadiumId = team.StadiumId,
            Conference = team.Conference,
            Division = team.Division,
            PrimaryColor = team.PrimaryColor,
            SecondaryColor = team.SecondaryColor,
            TertiaryColor = team.TertiaryColor,
            QuaternaryColor = team.QuaternaryColor,
            WikipediaLogoUrl = team.WikipediaLogoUrl,
            WikipediaWordMarkUrl = team.WikipediaWordMarkUrl,
            GlobalTeamId = team.GlobalTeamId,
            NbaDotComTeamId = team.NbaDotComTeamId,
            UpdatedBy = team.UpdatedBy,
            RefreshDate = team.RefreshDate,
            Players = team.Players.ToList()
        };

        public static PlayerDto AsDto(this Player player)
        => new()
        {
            PlayerId = player.PlayerId,
            IsApiId = player.IsApiId,
            SportsDataId = player.SportsDataId,
            Status = player.Status,
            TeamId = player.TeamId,
            Team = player.Team,
            Jersey = player.Jersey,
            PositionCategory = player.PositionCategory,
            Position = player.Position,
            FirstName = player.FirstName,
            LastName = player.LastName,
            Height = player.Height,
            Weight = player.Weight,
            BirthDate = player.BirthDate,
            BirthCity = player.BirthCity,
            BirthState = player.BirthState,
            BirthCountry = player.BirthCountry,
            HighSchool = player.HighSchool,
            College = player.College,
            Salary = player.Salary,
            PhotoUrl = player.PhotoUrl,
            Experience = player.Experience,
            InjuryStatus = player.InjuryStatus,
            InjuryBodyPart = player.InjuryBodyPart,
            InjuryStartDate = player.InjuryStartDate,
            InjuryNotes = player.InjuryNotes,
            DepthChartPosition = player.DepthChartPosition,
            DepthChartOrder = player.DepthChartOrder,
            UsaTodayPlayerId = player.UsaTodayPlayerId,
            UsaTodayHeadshotUrl = player.UsaTodayHeadshotUrl,
            UsaTodayHeadshotNoBackgroundUrl = player.UsaTodayHeadshotNoBackgroundUrl,
            UsaTodayHeadshotUpdated = player.UsaTodayHeadshotUpdated,
            UsaTodayHeadshotNoBackgroundUpdated = player.UsaTodayHeadshotNoBackgroundUpdated,
            NbaDotComPlayerId = player.NbaDotComPlayerId,
            UpdatedBy = player.UpdatedBy,
            RefreshDate = player.RefreshDate
        };
    }
}