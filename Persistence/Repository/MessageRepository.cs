using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        private readonly AnnouncementContext _context;
        public MessageRepository(AnnouncementContext context): base(context) 
        {
            _context = context;
        }

        public async Task<Message> GetByIdWithRolesAsync(int id)
        {
            var result = await _context
                .Set<Message>()
                .Include(x=>x.Roles)
                .FirstOrDefaultAsync(x => x.Id == id && x.Roles.Any(x => x.Id == 2));

            return result;
        }
    }
}
