using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DataAccess;
using API.Dtos;
using AutoMapper;

namespace API.BusinessLogic
{
    public class UserBusinessLogic : IUserBusinessLogic
    {
        private IUserDataAccess _userRepo;
        private IMapper _mapper;

        public UserBusinessLogic(IUserDataAccess userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<FormattedUserDto> CreateAsync(UserDto user)
        {
            var entity = _mapper.Map<User>(user);
            var newEntity = await _userRepo.CreateAsync(entity);
            return _mapper.Map<FormattedUserDto>(newEntity);
        }

        public async Task DeleteAsync(string emailAddress)
        {
            var users = await _userRepo.GetAsync(emailAddress);
            //since i'm not inforcing unqiue emails
            users.ToList().ForEach(async x => await _userRepo.DeleteAsync(x));
        }

        public async Task<IEnumerable<FormattedUserDto>> GetAsync(string emailAddress)
        {
            var entities = await _userRepo.GetAsync(emailAddress);
            return entities.Select(_mapper.Map<FormattedUserDto>);
        }
    }
}