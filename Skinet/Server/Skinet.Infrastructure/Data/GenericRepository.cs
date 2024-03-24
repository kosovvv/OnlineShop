using Microsoft.EntityFrameworkCore;
using Skinet.Core.Entities;
using Skinet.Core.Interfaces;
using Skinet.Core.Specification;

namespace Skinet.Infrastructure.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext context;
        public GenericRepository(StoreContext context)
        {
            this.context = context;
        }

        public void Add(T entity)
        {
            context.Set<T>().Add(entity); 
        }

        public Task<int> CountAsync(ISpecification<T> specification)
        {
            return ApplySpecification(specification).CountAsync();
        }

        public void Delete(T entity)
        {
            context.Set<T>().Remove(entity);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await context.Set<T>().FindAsync(id);
        }

        public async Task<T> GetEntityWithSpec(ISpecification<T> specification)
        {
            return await ApplySpecification(specification).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> LislAllAsync()
        {
            return await context.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> ListAsync(ISpecification<T> specification)
        {
            return await ApplySpecification(specification).ToListAsync();
        }

        public void Update(T entity)
        {
            context.Set<T>().Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(context.Set<T>().AsQueryable(), spec);
        }
    }
}
