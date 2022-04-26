using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NbaStats.Application.Abstractions;
using NbaStats.Application.DTO;
using NbaStats.Application.Queries;
using NbaStats.Domain.Entities;

namespace NbaStats.Web.Pages
{
    [BindProperties]
    public class IndexModel : PageModel
    {
        [BindProperty(SupportsGet = true)] 
        public DateTime? Date { get; set; }
        
        private readonly ILogger<IndexModel> _logger;
        private readonly IDispatcher _dispatcher;
        public List<IndexPageGameDto> Games;


        public IndexModel(ILogger<IndexModel> logger, IDispatcher dispatcher)
        {
            _logger = logger;
            _dispatcher = dispatcher;
        }
        
        public async Task OnGet(DateTime? date)
        {
            Date = date ?? DateTime.Now;
            Games = _dispatcher.QueryAsync(new GetGamesByDate {Date = Date.Value }).Result.Select(x => new IndexPageGameDto(x)).ToList();
            foreach (var item in Games)
            {
                item.HomeTeamDto = await _dispatcher.QueryAsync(new GetTeam {TeamId = item.HomeTeamId.Value});
                item.AwayTeamDto = await _dispatcher.QueryAsync(new GetTeam {TeamId = item.AwayTeamId.Value});
            }
        }
    }
}

