using System.Linq.Expressions;

namespace KoiCare.Application.Abtractions.Database
{
    public interface IRepository<T> where T : class
    {
        T Add(T entity);
        void Update(T entity);
        void Remove(T entity);
        IQueryable<T> Queryable();
        IQueryable<T> QueryableInclude(params Expression<Func<T, object>>[] includes);
        T? Find(int id);
        Task AddAsync(T entity, CancellationToken cancellationToken);
    }
}
