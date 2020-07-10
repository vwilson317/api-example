using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.DataAccess
{
    public class UserDataAccess : IUserDataAccess
    {
        public async Task<User> CreateAsync(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<User>> GetAsync(string emailAddress)
        {
            throw new NotImplementedException();
        }
    }
}
