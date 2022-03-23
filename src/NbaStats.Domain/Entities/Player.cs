using System;

namespace NbaStats.Domain.Entities
{
    public class Player
    {
        public int PlayerId { get; set; }
        public int? ApiPlayerId { get; set; }
        public string SportsDataId { get; set; }
        public string Status { get; set; }
        public int? TeamId { get; set; }
        public int? ApiTeamId { get; set; }
        public string Team { get; set; }
        public int? Jersey { get; set; }
        public string PositionCategory { get; set; }
        public string Position { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Height { get; set; }
        public int? Weight { get; set; }
        public DateTime? BirthDate { get; set; }
        public string BirthCity { get; set; }
        public string BirthState { get; set; }
        public string BirthCountry { get; set; }
        public string HighSchool { get; set; }
        public string College { get; set; }
        public int? Salary { get; set; }
        public string PhotoUrl { get; set; }
        public int? Experience { get; set; }
        public string SportRadarPlayerId { get; set; }
        public int? RotoworldPlayerId { get; set; }
        public int? RotoWirePlayerId { get; set; }
        public int? FantasyAlarmPlayerId { get; set; }
        public int? StatsPlayerId { get; set; }
        public int? SportsDirectPlayerId { get; set; }
        public int? XmlTeamPlayerId { get; set; }
        public string InjuryStatus { get; set; }
        public string InjuryBodyPart { get; set; }
        public DateTime? InjuryStartDate { get; set; }
        public string InjuryNotes { get; set; }
        public int? FanDuelPlayerId { get; set; }
        public int? DraftKingsPlayerId { get; set; }
        public int? YahooPlayerId { get; set; }
        public string FanDuelName { get; set; }
        public string DraftKingsName { get; set; }
        public string YahooName { get; set; }
        public string DepthChartPosition { get; set; }
        public int? DepthChartOrder { get; set; }
        public int? GlobalTeamId { get; set; }
        public string FantasyDraftName { get; set; }
        public int? FantasyDraftPlayerId { get; set; }
        public int? UsaTodayPlayerId { get; set; }
        public string UsaTodayHeadshotUrl { get; set; }
        public string UsaTodayHeadshotNoBackgroundUrl { get; set; }
        public DateTime? UsaTodayHeadshotUpdated { get; set; }
        public DateTime? UsaTodayHeadshotNoBackgroundUpdated { get; set; }
        public int? NbaDotComPlayerId { get; set; }

        public virtual Team TeamNavigation { get; set; }
    }
}