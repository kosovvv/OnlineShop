using OnlineShop.Data.Common.Repositories;

namespace OnlineShop.Data.Common
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> GetRepository<T>() where T : class;
        Task Save();
    }
}
