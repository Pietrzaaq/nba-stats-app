namespace NbaStats.Domain.Exceptions
{
    public class InvalidOrUnsupportedStatusException: NbaStatsException
    {
        public string Value { get; }

        public InvalidOrUnsupportedStatusException(string value) : base(message: $"Status value: {value} is either null or unsupported")
        {
            Value = value;
        }

    }
}