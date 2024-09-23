namespace KoiCare.Application.Abtractions.Database
{
    public interface IRepository<T> where T : class
    {
        T Add(T entity);
        void Update(T entity);
        void Remove(T entity);
        IQueryable<T> Queryable();
        T? Find(int id);
    }
}
