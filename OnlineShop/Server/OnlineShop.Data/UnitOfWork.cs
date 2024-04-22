using OnlineShop.Data.Common.Models;
using OnlineShop.Data.Common.Repositories;
using OnlineShop.Data.Repositories;

namespace OnlineShop.Data.Common
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext context;
        private readonly Dictionary<Type, object> repositories = new Dictionary<Type, object>();

        public UnitOfWork(StoreContext context)
        {
            this.context = context;
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            var type = typeof(T);
            if (!repositories.ContainsKey(type))
            {
                IRepository<T> repository;
                if (typeof(IDeletableEntity).IsAssignableFrom(type))
                {
                    repository = (IRepository<T>)Activator.CreateInstance(typeof(EfDeletableEntityRepository<>).MakeGenericType(type), context);
                }
                else
                {
                    repository = new EfRepository<T>(context);
                }

                repositories[type] = repository;
            }
            return (IRepository<T>)repositories[type];
        }


        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            if (context != null)
            {
                context.Dispose();
            }
            GC.SuppressFinalize(this);
        }
    }
}
