using Glow_Up.Core.Repositories;
using Glow_Up.Core.Specifications;
using Glow_Up.Repositories._Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Repositories.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec, CancellationToken cancellationToken)
        {
            return await ApplySecifications(spec).ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.Set<T>().AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<T?> GetByIdWithSpecAsync(ISpecification<T> spec, CancellationToken cancellationToken)
        {
            return await ApplySecifications(spec).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _dbContext.Set<T>().FindAsync(new object[] { id }, cancellationToken);
        }

        public void Add(T entity)
        => _dbContext.Set<T>().Add(entity);

        public void Update(T entity)
        => _dbContext.Set<T>().Update(entity);

        public void Delete(T entity)
        => _dbContext.Set<T>().Remove(entity);

        private IQueryable<T> ApplySecifications(ISpecification<T> spec)
        {
            return SpecificationsEvaluator<T>.GetQuery(_dbContext.Set<T>(), spec);
        }

        public Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        {
            return _dbContext.Set<T>().CountAsync(predicate, cancellationToken);
        }

        public Task<int> CountAsync(CancellationToken cancellationToken)
        {
            return _dbContext.Set<T>().CountAsync(cancellationToken);
        }

        public Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        {
            return _dbContext.Set<T>().FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public async Task<int> GetCountAsync(ISpecification<T> spec, CancellationToken cancellationToken)
        {
            return await ApplySecifications(spec).CountAsync(cancellationToken);
        }

        public IQueryable<T> GetAllAsQueryable(ISpecification<T>? spec = null)
        {
            var query = _dbContext.Set<T>().AsQueryable();

            if (spec != null)
            {
                query = SpecificationsEvaluator<T>.GetQuery(query, spec);
            }

            return query; // Ensure IQueryable<T> is returned
        }

        public IQueryable<T> GetAllAsQueryable()
        {
            return _dbContext.Set<T>().AsQueryable();
        }

        public async Task AddRange(IEnumerable<T> entities)
        => await _dbContext.Set<T>().AddRangeAsync(entities);
    }
}
