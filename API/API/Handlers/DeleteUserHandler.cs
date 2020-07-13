using System.Threading;
using System.Threading.Tasks;
using API.BusinessLogic;
using API.Commands;
using API.Dtos;
using MediatR;

namespace API.Handlers
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand>
    {
        private IUserBusinessLogic _userBusinessLogic;

        public DeleteUserHandler(IUserBusinessLogic userBusinessLogic)
        {
            _userBusinessLogic = userBusinessLogic;
        }

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            await _userBusinessLogic.DeleteAsync(request.EmailAddress);
            return new Unit();
        }
    }
}
