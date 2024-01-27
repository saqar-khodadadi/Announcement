using Domain.Entities.Base;
using Domain.Repositories.Base;
using Persistence.Data;

namespace Persistence.Repository.Base
{
    public class Repository<T> : RepositoryBase<T, int>, IRepository<T>
        where T : class, IEntityBase<int>
    {
        public Repository(AnnouncementContext context)
            : base(context)
        {
        }
    }
}
