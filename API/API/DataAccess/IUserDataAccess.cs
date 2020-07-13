using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.DataAccess
{
    public interface IUserDataAccess
    {
        Task<IEnumerable<User>> GetAsync(string emailAddress);
        Task<User> CreateAsync(User user);
        Task DeleteAsync(User user);
    }
}
