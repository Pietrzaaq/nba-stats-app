using System.Collections.Generic;
using NbaStats.Domain.Exceptions;

namespace NbaStats.Domain.ValueObjects
{
    public sealed class Status
    {
        public static readonly HashSet<string> AllowedValues = new()
        {
            "Active", "Finished", "Scheduled", "Closed"
        };

        public string Value;
        
        public Status(string value)
        {
            SetStatus(value);
        }

        public void SetStatus(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length < 6 || !AllowedValues.Contains(value))
            {
                throw new InvalidOrUnsupportedStatusException(value);
            }

            Value = value;
        }
        

        public override string ToString() => Value;
    }
}