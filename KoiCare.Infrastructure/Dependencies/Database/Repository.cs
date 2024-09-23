using KoiCare.Application.Abtractions.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace KoiCare.Infrastructure.Dependencies.Database
{
    public class Repository<T>(AppDbContext context) : IRepository<T> where T : class
    {
        public T Add(T entity)
        {
            return context.Set<T>().Add(entity).Entity;
        }

        public async Task AddAsync(T entity, CancellationToken cancellationToken)
        {
            await context.Set<T>().AddAsync(entity, cancellationToken);
        }

        public T? Find(int id)
        {
            return context.Set<T>().Find(id);
        }

        public IQueryable<T> Queryable()
        {
            return context.Set<T>().AsQueryable();
        }

        public IQueryable<T> QueryableInclude(params Expression<Func<T, object>>[] includes)
        {
            return includes.Aggregate(context.Set<T>().AsQueryable(), (current, include) => current.Include(include));
        }

        public void Remove(T entity)
        {
            context.Set<T>().Remove(entity);
        }

        public void Update(T entity)
        {
            context.Set<T>().Update(entity);
        }
    }
}
