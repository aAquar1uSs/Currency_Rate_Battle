using System.Net;
using CurrencyRateBattleServer.Dto;
using CurrencyRateBattleServer.Helpers;
using CurrencyRateBattleServer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CurrencyRateBattleServer.Controllers;

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

        var tokens = await _accountService.GetUserAsync(userData);

        return tokens is null ? Unauthorized("No such user exists. Try again") : Ok(tokens.Token);
    }

    [HttpPost("registration")]
    [AllowAnonymous]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreateUserAsync([FromBody] UserDto userData)
    {
        if (!ModelState.IsValid)
            return BadRequest("Invalid data. Please try again.");

        try
        {
            _logger.LogDebug("Registration was triggered.");
            var tokens = await _accountService.СreateUserAsync(userData);

            if (tokens is null)
                return NotFound();

            return Ok(tokens.Token);
        }
        catch (CustomException ex)
        {
            // return error message if there was an exception
            return BadRequest(new {message = ex.Message});
        }
        catch (DbUpdateException)
        {
            _logger.LogDebug("An unexpected error occurred. When try update data in the database");
            return BadRequest("An unexpected error occurred. Please try again.");
        }
    }

    [HttpGet("user-profile")]
    [Authorize]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetUserInfoAsync()
    {
        var guidId = _accountService.GetGuidFromRequest(HttpContext);
        if (guidId is null)
            return BadRequest();

        var result = await _accountService.GetAccountInfoAsync((Guid)guidId);
        return Ok(result);
    }
}
