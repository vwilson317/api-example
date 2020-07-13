using System;
using System.Collections.Generic;
using API.BusinessLogic;
using API.Dtos;
using MediatR;

namespace API.Query
{

    public class GetUsersByEmailQuery : IRequest<IEnumerable<FormattedUserDto>>
    {
        public string EmailAddress { get; private set; }

        public GetUsersByEmailQuery(string emailAddress)
        {
            EmailAddress = emailAddress;
        }
    }
}
