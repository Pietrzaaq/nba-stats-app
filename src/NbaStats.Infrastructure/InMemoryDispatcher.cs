using System.Threading.Tasks;
using NbaStats.Application.Abstractions;

namespace NbaStats.Infrastructure
{
    public class InMemoryDispatcher: IDispatcher
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public InMemoryDispatcher(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        public Task SendAsync<TCommand>(TCommand command) where TCommand : class, ICommand
        {
            return _commandDispatcher.SendAsync(command);
        }

        public Task<TResult> QueryAsync<TResult>(IQuery<TResult> query)
        {
            return _queryDispatcher.QueryAsync(query);
        }
    }
}