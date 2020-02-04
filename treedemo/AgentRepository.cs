using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace treedemo
{
    public class AgentRepository<T> : IRepository<T> where T : class, new()
    {
        protected readonly AgentContext Context;

        public AgentRepository(AgentContext context)
        {
            Context = context;
        }

        public IQueryable<T> Table => Context.Set<T>().AsQueryable();

        public IQueryable<T> TableNoTracking => Context.Set<T>().AsNoTracking();

        public int Delete(T entity)
        {
            try
            {
                Context.Set<T>().Remove(entity);
                return 1;
            }
            catch (Exception)
            {

                return 0;
            }
        }

        public int DeleteWhere(Expression<Func<T, bool>> criteria)
        {
            try
            {
                IQueryable<T> entities = Context.Set<T>().Where(criteria);
                foreach (var entity in entities)
                {
                    Context.Entry(entity).State = EntityState.Deleted;
                }
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }

        }

        public T GetById(object id)
        {
            return Context.Set<T>().Find(id);
        }

        public int Insert(T entity)
        {
            try
            {
                Context.Set<T>().Add(entity);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }

        }

        public int InsertMany(IEnumerable<T> list)
        {
            try
            {
                Context.Set<T>().AddRange(list);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int Update(T entity)
        {
            try
            {
                Context.Entry(entity).State = EntityState.Modified;
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }

}
