using KoiCare.Application.Abtractions.Database;

namespace KoiCare.Infrastructure.Dependencies.Database
{
    public class UnitOfWork(AppDbContext context) : IUnitOfWork
    {
        private readonly AppDbContext context = context;
        private readonly Dictionary<Type, object> repositories = [];
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IRepository<T> Repository<T>() where T : class
        {
            if (repositories.ContainsKey(typeof(T)))
            {
                return (IRepository<T>)repositories[typeof(T)];
            }

            var repository = new Repository<T>(context);
            repositories.Add(typeof(T), repository);

            return repository;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}
