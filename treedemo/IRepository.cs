using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace treedemo
{
    public interface IRepository<T> where T : class, new()
    {
        IQueryable<T> Table { get; }

        IQueryable<T> TableNoTracking { get; }
        T GetById(object id);
        int Insert(T entity);
        int InsertMany(IEnumerable<T> list);
        int Update(T entity);
        int Delete(T entity);
        int DeleteWhere(Expression<Func<T, bool>> criteria);
    }

}
