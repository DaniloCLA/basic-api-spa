using System.Threading.Tasks;

using fluxo.DATA.Models;
using fluxo.DATA.Helpers;
using fluxo.DATA.Params;

namespace fluxo.DATA.Repository
{
    public interface IUserRepository : IBaseRepository
    {         
         Task<User> GetUser(int id);
         Task<PagedList<User>> GetUsers(UserListParams userParams);
    }
}