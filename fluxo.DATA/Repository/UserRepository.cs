using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using fluxo.DATA.Context;
using fluxo.DATA.Helpers;
using fluxo.DATA.Models;
using fluxo.DATA.Params;
using System.Linq;

namespace fluxo.DATA.Repository
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(DataContext context) : base(context) {}

        public async Task<User> GetUser(int id)
        {
            return await _context.Users
                .Include(u => u.OrganizationOwned)
                .Include(u => u.TeamsAssigned)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<PagedList<User>> GetUsers(UserListParams userParams)
        {
            var userRequesting = await _context.Users.FirstOrDefaultAsync(u => u.Id == userParams.UserId);

            var users = _context.Users
                .Include(p => p.OrganizationOwned)
                .Include(p => p.Organization)
                .Include(p => p.TeamsAssigned)
                .OrderByDescending(u => u.FullName).AsQueryable();

            users = users.Where(u => u.Id != userParams.UserId);
            users = users.Where(u => u.OrganizationId == userRequesting.OrganizationId);

            if (!string.IsNullOrEmpty(userParams.OrderBy))
            {
                switch (userParams.OrderBy)
                {
                    case "created":
                        users = users.OrderByDescending(u => u.Created);
                        break;
                    case "lastActive":
                        users = users.OrderByDescending(u => u.LastActive);
                        break;
                    default:
                        users = users.OrderByDescending(u => u.FullName);
                        break;
                }
            }

            return await PagedList<User>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
        }
    }
}