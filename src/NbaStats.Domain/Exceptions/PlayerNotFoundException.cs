namespace NbaStats.Domain.Exceptions
{
    public class PlayerNotFoundException : NbaStatsException
    {
        public int PlayerId { get; }
        
        public PlayerNotFoundException(int playerId) : base(message:$"Game with id: {playerId} not found.")
        {
            PlayerId = playerId;
        }
    }
}