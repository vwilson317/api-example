using System;
using System.Linq;
using System.Threading.Tasks;
using API.BusinessLogic;
using API.Commands;
using API.Dtos;
using API.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //using email address as resource primary key for querying data
        [HttpGet("{emailAddress}")]
        public async Task<IActionResult> Get(string emailAddress)
        {
            var query = new GetUsersByEmailQuery(emailAddress);
            var data = await _mediator.Send(query);
            if (data.Any())
            {
                return Ok(data);
            }
            else
            {
                return NoContent();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]UserDto user)
        {
            var command = new CreateUserCommand(user);
            var data = await _mediator.Send(command);
            if (data != null)
            {
                return Created(new Uri($"http://{HttpContext.Request.Host.Value}/api/users/{data.EmailAddress}"), data);
            }

            return BadRequest();
        }
    }
}
