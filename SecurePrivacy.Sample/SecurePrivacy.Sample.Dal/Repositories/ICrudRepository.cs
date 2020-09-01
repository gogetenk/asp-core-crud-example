using System.Threading.Tasks;

namespace SecurePrivacy.Sample.Dal.Repositories
{
    public interface ICrudRepository<T>
    {
        Task<T> Insert(T entity);
        Task<T> GetById(string id);
        Task<bool> Update(string id, T entity);
        Task Delete(string id);
    }
}
