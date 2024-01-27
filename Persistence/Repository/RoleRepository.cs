using Domain.Entities;
using Domain.Repositories;
using Persistence.Data;
using Persistence.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        private readonly AnnouncementContext _context;
        public RoleRepository(AnnouncementContext context) : base(context)
        {
            _context = context;
        }
    }
}
