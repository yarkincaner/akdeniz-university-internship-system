using Internships.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Internships.Core.Interfaces.Repositories
{
    public interface IUserRepositoryAsync : IGenericRepositoryAsync<User>
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User> GetByUserId(string userId);
    }
}
