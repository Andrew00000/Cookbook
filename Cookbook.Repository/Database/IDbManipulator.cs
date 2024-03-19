using System.Data;

namespace Cookbook.Repository.Database
{
    public interface IDbManipulator
    {
        Task<T> RunQuery<T>(Func<IDbConnection, Task<T>> callback, CancellationToken token);
        Task RunQuery(Func<IDbConnection, Task> callback, CancellationToken token);
    }
}
