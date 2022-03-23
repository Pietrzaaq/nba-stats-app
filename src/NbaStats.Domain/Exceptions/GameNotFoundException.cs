namespace NbaStats.Domain.Exceptions
{
    public class GameNotFoundException : NbaStatsException
    {
        public int GameId { get; }

        public GameNotFoundException(int gameId) : base(message:$"Game with id: {gameId} not found.")
        {
            GameId = gameId;
        }
    }
}