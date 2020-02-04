using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace treedemo
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext, new()
    {
        private readonly TContext _context;

        public UnitOfWork(TContext context)
        {
            _context = context;
        }


        public bool Save()
        {
            return _context.SaveChanges() >= 0;
        }

        public bool Save(bool acceptAllChangesOnSuccess)
        {

            return _context.SaveChanges(acceptAllChangesOnSuccess) >= 0;

        }

        public async Task<bool> SaveAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {

            return await _context.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken) >= 0;

        }

        public async Task<bool> SaveAsync(CancellationToken cancellationToken = default(CancellationToken))
        {

            return await _context.SaveChangesAsync(cancellationToken) >= 0;

        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }

}
