using CHS.BLL.Interfaces;
using CHS.DAL;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CHS.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {

        private readonly CreditHoursSystemContext context;

        public GenericRepository(CreditHoursSystemContext context)
        {
            this.context = context;
        }

        public T Add(T entity)
        {
            context.Set<T>().Add(entity);
            context.SaveChanges();
            return entity;
        }

        public IEnumerable<T> AddRange(IEnumerable<T> entities)
        {
            context.Set<T>().AddRange(entities);
            context.SaveChanges();
            return entities;
        }

        public void Attach(T entity) => context.Set<T>().Attach(entity);

        public int Count() => context.Set<T>().Count();

        public int Count(Expression<Func<T, bool>> criteria) => context.Set<T>().Count(criteria);

        public void Delete(T entity) => context.Set<T>().Remove(entity);

        public T Find(Expression<Func<T, bool>> criteria) => context.Set<T>().SingleOrDefault(criteria);

        public T Find(Expression<Func<T, bool>> criteria, string[] Includes = null)
        {
            IQueryable<T> query = context.Set<T>();
            if (Includes != null)
            {
                foreach (var include in Includes)
                {
                    query = query.Include(include);
                }
            }
            return query.SingleOrDefault(criteria);
        }
        public IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, string[] Includes = null)
        {
            IQueryable<T> query = context.Set<T>();
            if (Includes != null)
            {
                foreach (var include in Includes)
                {
                    query = query.Include(include);
                }
            }
            return query.Where(criteria).ToList();
        }

        public IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, int skip, int take) =>
            context.Set<T>().Where(criteria).Skip(skip).Take(take).ToList();

        public IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, int? skip, int? take,
            Expression<Func<T, object>>? orderBy = null, string orderByDirection = "ASC")
        {
            IQueryable<T> query = context.Set<T>().Where(criteria);
            if (take.HasValue) { query.Take(take.Value); }
            if (skip.HasValue) { query.Skip(skip.Value); }
            if (orderBy != null)
            {
                if (orderByDirection == OrderBy.Ascending)
                { query = query.OrderBy(orderBy); }
                else
                { query = query.OrderByDescending(orderBy); }
            }
            return query.ToList();
        }

        public IEnumerable<T> GetAll() => context.Set<T>().ToList();

        public T GetById(int id) => context.Set<T>().Find(id);

        public T Update(T entity)
        {
            context.Set<T>().Update(entity);
            return entity;
        }
      
    }
}
