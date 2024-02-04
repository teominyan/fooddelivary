using System.Threading.Tasks;
using fooddelivary.Shared.Domain;

namespace fooddelivary.Server.IRepository
{
    public interface IUnitOfWork
    {
        IGenericRepository<Product> ProductRepository { get; }
        IGenericRepository<Order> OrderRepository { get; }

        Task CommitAsync();
        void Rollback();
    }
}
