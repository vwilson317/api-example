using System.Threading;
using System.Threading.Tasks;
using API.BusinessLogic;
using API.Commands;
using API.Dtos;
using MediatR;

namespace API.Handlers
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, FormattedUserDto>
    {
        private IUserBusinessLogic _userBusinessLogic;

        public CreateUserHandler(IUserBusinessLogic userBusinessLogic)
        {
            _userBusinessLogic = userBusinessLogic;
        }

        public async Task<FormattedUserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var data = await _userBusinessLogic.CreateAsync(request.User);
            return data;
        }
    }
}
