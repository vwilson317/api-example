using MediatR;

namespace API.Commands
{
    public class DeleteUserCommand : IRequest
    {
        public string EmailAddress { get; private set; }

        public DeleteUserCommand(string emailAddress)
        {
            EmailAddress = emailAddress;
        }
    }
}
