using System.Threading.Tasks;

namespace fluxo.DATA.Repository
{
    public interface IBaseRepository
    {
         void Add<T>(T entity) where T: class;
         void Delete<T>(T entity) where T: class;
         Task<bool> SaveAll();
         
    }
}