using Domain.Entities;
using Domain.Repositories.Base;
using System.Security.Cryptography;

namespace Domain.Repositories
{
    public interface IMessageRepository: IRepository<Message>
    {
        Task<Message> GetByIdWithRolesAsync(int id);
    }
}
