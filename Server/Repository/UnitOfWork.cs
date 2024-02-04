using System.Threading.Tasks;
using fooddelivary.Server.IRepository;
using Microsoft.EntityFrameworkCore;
using fooddelivary.Shared.Domain;
using fooddelivary.Server.Data;

namespace fooddelivary.Server.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            ProductRepository = new GenericRepository<Product>(_context);
            OrderRepository = new GenericRepository<Order>(_context);
 
        }

        public IGenericRepository<Product> ProductRepository { get; private set; }
        public IGenericRepository<Order> OrderRepository { get; private set; }


        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Rollback()
        {
            _context.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
        }
    }
}