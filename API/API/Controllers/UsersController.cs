using System;
using System.Linq;
using System.Threading.Tasks;
using API.BusinessLogic;
using API.Dtos;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserBusinessLogic _userBusinessLogic;

        public UsersController(IUserBusinessLogic userBusinessLogic)
        {
            _userBusinessLogic = userBusinessLogic;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = await _userBusinessLogic.GetAsync();
            if (data.Any())
            {
                return Ok();
            }
            else
            {
                return NoContent();
            }
        }

        [HttpGet("{emailAddress}")]
        public async Task<IActionResult> Get(string emailAddress)
        {
            var data = await _userBusinessLogic.GetAsync(emailAddress);
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
            var data = await _userBusinessLogic.CreateAsync(user);
            if(data != null)
            {
                return Created(new Uri($"http://localhost:3000/api/users/{data.EmailAddress}"), data);
            }

            return new EmptyResult();
        }
    }
}
