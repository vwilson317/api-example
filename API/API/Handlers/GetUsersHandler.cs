using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using API.BusinessLogic;
using API.Dtos;
using API.Query;
using MediatR;

namespace API.Handlers
{
    public class GetUsersHandler : IRequestHandler<GetUsersByEmailQuery, IEnumerable<FormattedUserDto>>
    {
        private IUserBusinessLogic _userBusinessLogic;

        public GetUsersHandler(IUserBusinessLogic userBusinessLogic)
        {
            _userBusinessLogic = userBusinessLogic;
        }

        public async Task<IEnumerable<FormattedUserDto>> Handle(GetUsersByEmailQuery request, CancellationToken cancellationToken)
        {
            var data = await _userBusinessLogic.GetAsync(request.EmailAddress);
            return data;
        }
    }
}
