using Skinet.Core.Entities;
using Skinet.Core.Interfaces;
using System.Collections;

namespace Skinet.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext storeContext;

        private Hashtable repositories;

        public UnitOfWork(StoreContext storeContext)
        {
            this.storeContext = storeContext;
        }
        public async Task<int> Complete()
        {
            return await storeContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            storeContext.Dispose();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            if (repositories == null)
            {
                repositories = new Hashtable();
            }

            var type = typeof(TEntity).Name;

            if (!repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<>);

                var repoInstance =
                    Activator.CreateInstance
                    (repositoryType.MakeGenericType(typeof(TEntity)), storeContext);

                repositories.Add(type, repoInstance);
            }
            return (IGenericRepository<TEntity>)repositories[type];
        }
    }
}
