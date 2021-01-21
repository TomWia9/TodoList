using System.Threading.Tasks;

namespace TodoList.Server.Repositories
{
    public interface IDbRepository
    {
        void Add<T>(T entity) where T : class;
        void Remove<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();
    }
}
