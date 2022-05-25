using System.Net;
using CurrencyRateBattleServer.Dto;
using CurrencyRateBattleServer.Helpers;
using CurrencyRateBattleServer.Models;
using CurrencyRateBattleServer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CurrencyRateBattleServer.Controllers;

[Route("api/rates")]
[ApiController]
[Authorize]
public class RateController : ControllerBase
{
    private readonly ILogger<RoomController> _logger;

    private readonly IRateService _rateService;

    private readonly IAccountService _accountService;

    public RateController(ILogger<RoomController> logger,
        IRateService rateService,
        IAccountService accountService)
    {
        _logger = logger;

        _rateService = rateService;

        _accountService = accountService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Room>>> GetRatesAsync(bool? isActive, string? currencyCode)
    {
        _logger.LogDebug("List of rates are retrieving.");
        var rates = await _rateService.GetRatesAsync(isActive, currencyCode);
        return Ok(rates);
    }

    // GET api/rates/
    [HttpGet("get-user-bets")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> GetUserBetsAsync()
    {
        var userId = _accountService.GetGuidFromRequest(HttpContext);
        if (userId is null)
            return BadRequest();

        var account = await _accountService.GetAccountByUserIdAsync(userId);
        if (account is null)
            return BadRequest();

        var bets = await _rateService.GetRatesByAccountIdAsync(account.Id);
        return Ok(bets);
    }

    // GET api/rates/
    [HttpGet("get-users-rating")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> GetUsersRatingAsyn()
    {
        return Ok(await _rateService.GetUsersRatingAsyn());
    }


    [HttpPost]
    public async Task<IActionResult> CreateRateAsync([FromBody] Rate rateToCreate)
    {
        _logger.LogDebug("New rate creation is trigerred.");
        try
        {
            var rate = await _rateService.CreateRateAsync(rateToCreate);
            _logger.LogInformation($"Rate has been created successfully ({rate.Id})");
            return Ok(rate);
        }
        catch (CustomException ex)
        {
            // return error message if there was an exception
            return BadRequest(new { message = ex.Message });
        }
        catch (DbUpdateException)
        {
            _logger.LogDebug("An unexpected error occurred during the attempt to create a rate in the DB.");
            return BadRequest("An unexpected error occurred. Please try again.");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRateAsync(Guid id, [FromBody] Rate updatedRate)
    {
        try
        {
            _rateService.UpdateRateByRoomIdAsync(id, updatedRate);
            _logger.LogInformation($"Rate has been updated successfully ({id})");
            return Ok();
        }
        catch (CustomException ex)
        {
            // return error message if there was an exception
            return BadRequest(new { message = ex.Message });
        }
        catch (DbUpdateException)
        {
            _logger.LogDebug("An unexpected error occurred during the attempt to update the rate in the DB.");
            return BadRequest("An unexpected error occurred. Please try again.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRateAsync(Guid id)
    {
        try
        {
            await _rateService.DeleteRateAsync(id);
            _logger.LogInformation($"Rate has been deleted successfully ({id})");
            return Ok();
        }
        catch (CustomException ex)
        {
            // return error message if there was an exception
            return BadRequest(new { message = ex.Message });
        }
        catch (DbUpdateException)
        {
            _logger.LogDebug("An unexpected error occurred during the attempt to delete the rate in the DB.");
            return BadRequest("An unexpected error occurred. Please try again.");
        }
    }
}
