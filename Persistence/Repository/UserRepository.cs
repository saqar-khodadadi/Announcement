using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Repository.Base;

namespace Persistence.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly AnnouncementContext _context;
        public UserRepository(AnnouncementContext context) : base(context) 
        {
            _context = context;
        }

        public async Task<User> GetUserWithRoleByUsername(string username)
        {
            return await _context.Set<User>().Include(x=>x.Roles).FirstOrDefaultAsync(x => x.Username == username);
        }
    }
}
