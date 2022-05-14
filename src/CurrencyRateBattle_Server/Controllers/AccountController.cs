using System.Net;
using CurrencyRateBattle_Server.Dto;
using CurrencyRateBattle_Server.Models;
using CurrencyRateBattle_Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyRateBattle_Server.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;

        private readonly IAccountService _accountService;

        public AccountController(ILogger<AccountController> logger,
            IAccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }

        [HttpPost("login")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> LoginAsync([FromBody] UserDto userData)
        {
            var user = new User { Email = userData.Email, Password = userData.Password };
            var token = await _accountService.LoginAsync(user);

            if (token is null)
                return Unauthorized();

            return Ok(token);
        }


        [HttpPost("registration")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RegistrationAsync([FromBody] UserDto userData)
        {
            var user = new User { Email = userData.Email, Password = userData.Password };
            var token = await _accountService.RegistrationAsync(user);

            if (token is null)
                return BadRequest();

            return Ok(token);
        }
    }
}
