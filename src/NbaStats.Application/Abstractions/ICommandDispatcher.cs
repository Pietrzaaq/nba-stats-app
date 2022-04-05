using System.Threading.Tasks;

namespace NbaStats.Application.Abstractions
{
    public interface ICommandDispatcher
    {
        Task SendAsync<TCommand>(TCommand command) where TCommand : class, ICommand;
    }
}