using CHS.BLL.Interfaces;
using CHS.DAL;
namespace CHS.BLL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CreditHoursSystemContext context;
        private Dictionary<Type, object> repositories;

        public UnitOfWork(CreditHoursSystemContext context)
        {
            this.context = context;
        }

        public IGenericRepository<T> Repository<T>() where T : class
        {
            if (repositories == null)
            {
                repositories = new Dictionary<Type, object>();
            }

            var type = typeof(T);
            if (!repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), context);
                repositories.Add(type, repositoryInstance);
            }

            return (IGenericRepository<T>)repositories[type];
        }
        public async Task<int> Complete()
        {
            return await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
