using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DataAccess
{
    public class UserDataAccess : IUserDataAccess
    {
        public async Task<User> CreateAsync(User user)
        {
            user.UserId = Guid.NewGuid().ToString(); 
            return user;
        }

        public async Task<IEnumerable<User>> GetAsync(string emailAddress)
        {
            var users = new[]{
                new User{EmailAddress = "1"},
                new User{EmailAddress = "2"},
                new User{EmailAddress = "3"},
                new User{EmailAddress = "4"},
                new User{EmailAddress = "5"},
            }.ToList();

            return users;
        }
    }
}
