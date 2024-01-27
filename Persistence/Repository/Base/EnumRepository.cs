
using Domain.Entities.Base;
using Domain.Repositories.Base;
using Persistence.Data;

namespace Persistence.Repository.Base
{
    public class EnumRepository<T> : RepositoryBase<T, int>, IEnumRepository<T>
        where T : class, IEntityBase<int>
    {
        public EnumRepository(AnnouncementContext context)
            : base(context)
        {
        }
    }
}
