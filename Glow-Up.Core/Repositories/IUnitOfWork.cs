using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Core.Repositories
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        // Apply Open Colsed Principle 
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class;
        Task<int> CompleteAsync(CancellationToken cancellationToken = default);

    }
}
