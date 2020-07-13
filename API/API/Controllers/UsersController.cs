using System;
using System.Collections.Generic;
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
    //todo: replace with fluent validation and middleware to remove logic from controllers
    public abstract class AppControllerBase : ControllerBase
    {
        private IMediator _mediator;

        public AppControllerBase(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Get<TQuery, TData>(TQuery query)
        {
            if (!ModelState.IsValid)
            {
                //todo: pop with error messages
                return ValidationProblem();
            }

            try
            {
                var data = await _mediator.Send(query);
                if ((data as IEnumerable<TData>).Any())
                {
                    return Ok(data);
                }
                else
                {
                    return NoContent();
                }
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        public async Task<IActionResult> Post<TCommand, TData>(TCommand command)
            where TData : class, IEmailable
        {
            if (!ModelState.IsValid)
            {
                //todo: pop with error messages
                return ValidationProblem();
            }
            try
            {
                var data = await _mediator.Send(command);
                if(data != null)
                {
                    return Created(data as TData);
                }
                else
                {
                    //revist this
                    return NoContent();
                }
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{emailAddress}")]
        public async Task<IActionResult> Delete<TCommand>(TCommand command)
        {
            if (!ModelState.IsValid)
            {
                //todo: pop with error messages
                return ValidationProblem();
            }
            try
            {
                await _mediator.Send(command);

                return Accepted();
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        protected abstract IActionResult Created<T>(T obj)
            where T : IEmailable;
    }
    [Route("api/[controller]")]
    public class UsersController : AppControllerBase
    {
        public UsersController(IMediator mediator):base(mediator)
        {
        }

        //using email address as resource primary key for querying data
        [HttpGet("{emailAddress}")]
        public async Task<IActionResult> Get(string emailAddress)
        {
            var query = new GetUsersByEmailQuery(emailAddress);
            return await base.Get<GetUsersByEmailQuery, FormattedUserDto>(query);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]UserDto user)
        {
            var command = new CreateUserCommand(user);
            return await base.Post<CreateUserCommand, FormattedUserDto>(command);
        }

        [HttpDelete("{emailAddress}")]
        public async Task<IActionResult> Delete(string emailAddress)
        {
            var command = new DeleteUserCommand(emailAddress);
            return await base.Delete(command);
        }

        protected override IActionResult Created<T>(T obj)
        {
            return Created(new Uri($"http://{HttpContext.Request.Host.Value}/api/users/{obj.EmailAddress}"), obj);
        }
    }
}
