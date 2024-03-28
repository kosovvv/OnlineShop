using Core.Specifications;
using Skinet.Core.Entities;

namespace Skinet.Core.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> LislAllAsync();
        Task<T> GetEntityWithSpec(ISpecification<T> specification);
        Task<IEnumerable<T>> ListAsync(ISpecification<T> specification);
        Task<int> CountAsync(ISpecification<T> specification);

        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
