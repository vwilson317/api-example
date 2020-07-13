using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dtos;

namespace API.BusinessLogic
{
    public interface IUserBusinessLogic
    {
        Task<IEnumerable<FormattedUserDto>> GetAsync(string emailAddress);
        Task<FormattedUserDto> CreateAsync(UserDto user);
        Task DeleteAsync(string emailAddress);
    }
}