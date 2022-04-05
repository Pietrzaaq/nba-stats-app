using System.Threading.Tasks;

namespace NbaStats.Application.Abstractions
{
    public interface IQueryDispatcher
    {
        Task<TResult> QueryAsync<TResult>(IQuery<TResult> query);
    }
}