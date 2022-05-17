﻿using System.Net;
using CurrencyRateBattleServer.Dto;
using CurrencyRateBattleServer.Helpers;
using CurrencyRateBattleServer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CurrencyRateBattleServer.Controllers
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
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> LoginAsync([FromBody] UserDto userData)
        {
            _logger.LogDebug("Authentication was triggered.");
            if (!ModelState.IsValid)
                return BadRequest("Invalid data entered.");

            var token = await _accountService.LoginAsync(userData);

            if (token is null)
                return Unauthorized("This user does not exist. Please check the entered data and try again.");

            return Ok(token);
        }

        [HttpPost("registration")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> RegistrationAsync([FromBody] UserDto userData)
        {
            try
            {
                _logger.LogDebug("Registration was triggered.");
                var token = await _accountService.RegistrationAsync(userData);

                if (token is null)
                    return NotFound();

                return Ok(token);
            }
            catch (CustomException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });

            }
        }
    }
}
