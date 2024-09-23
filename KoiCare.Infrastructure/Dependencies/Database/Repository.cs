using KoiCare.Application.Abtractions.Database;

namespace KoiCare.Infrastructure.Dependencies.Database
{
    public class Repository<T>(AppDbContext context) : IRepository<T> where T : class
    {
        public T Add(T entity)
        {
            return context.Set<T>().Add(entity).Entity;
        }

        public T? Find(int id)
        {
            return context.Set<T>().Find(id);
        }

        public IQueryable<T> Queryable()
        {
            return context.Set<T>().AsQueryable();
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
