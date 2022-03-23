using System;

namespace NbaStats.Domain.Exceptions
{
    public abstract class NbaStatsException : Exception
    {
        protected NbaStatsException(string message) : base(message)
        {
            
        }
    }
}