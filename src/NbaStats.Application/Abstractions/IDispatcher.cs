using System.Threading.Tasks;

namespace NbaStats.Application.Abstractions
{
    public interface IDispatcher
    {
        Task SendAsync<TCommand>(TCommand command) where TCommand : class, ICommand;
        Task<TResult> QueryAsync<TResult>(IQuery<TResult> query);
    }
}