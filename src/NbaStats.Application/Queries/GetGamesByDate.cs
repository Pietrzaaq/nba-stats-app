using System;
using System.Collections.Generic;
using NbaStats.Application.Abstractions;
using NbaStats.Application.DTO;

namespace NbaStats.Application.Queries
{
    public class GetGamesByDate : IQuery<List<GameDto>>
    {
        public DateTime Date { get; set; }
    }
}