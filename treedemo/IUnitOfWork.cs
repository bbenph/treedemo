using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace treedemo
{
    public interface IUnitOfWork<TContext> where TContext : DbContext, new()
    {
        bool Save();
        bool Save(bool acceptAllChangesOnSuccess);
        Task<bool> SaveAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> SaveAsync(CancellationToken cancellationToken = default(CancellationToken));

    }
}
