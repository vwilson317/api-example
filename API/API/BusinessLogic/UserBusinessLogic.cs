using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dtos;

namespace API.BusinessLogic
{
    public class UserBusinessLogic : IUserBusinessLogic
    {
        public async Task<FormattedUserDto> CreateAsync(UserDto user)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<FormattedUserDto>> GetAsync()
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<FormattedUserDto>> GetAsync(string emailAddress)
        {
            throw new System.NotImplementedException();
        }
    }
}