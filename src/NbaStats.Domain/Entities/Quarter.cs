using System;

namespace NbaStats.Domain.Entities
{
    public partial class Quarter
    {
        public int QuarterId { get; set; }
        public int GameId { get; set; }
        public int? Number { get; set; }
        public string Name { get; set; }
        public int? AwayScore { get; set; }
        public int? HomeScore { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? RefreshDate { get; set; }

        public virtual Game Game { get; set; }
    }
}