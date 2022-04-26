using System;
using System.Collections.Generic;
using NbaStats.Domain.Entities;

namespace NbaStats.Application.DTO
{
    public class IndexPageGameDto : GameDto
    {
        public IndexPageGameDto(GameDto gameDto)
        {
            foreach (var prop in gameDto.GetType().GetProperties())
            {
                this.GetType().GetProperty(prop.Name).SetValue(this, prop.GetValue(gameDto, null), null);
            }
        }
        
        public TeamDto? HomeTeamDto { get; set; }
        public TeamDto? AwayTeamDto { get; set; }
    }
}