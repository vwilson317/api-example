using API.Dtos;
using MediatR;

namespace API.Commands
{
    public class CreateUserCommand : IRequest<FormattedUserDto>
    {
        public UserDto User { get; private set; }

        public CreateUserCommand(UserDto user)
        {
            User = user;
        }
    }
}
