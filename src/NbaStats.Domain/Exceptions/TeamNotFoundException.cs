namespace NbaStats.Domain.Exceptions
{
    public class TeamNotFoundException : NbaStatsException
    {
        public int Team { get; }
        
        public TeamNotFoundException(int teamId) : base(message:$"Game with id: {teamId} not found.")
        {
            Team = teamId;
        }
    }
}