using System.Net;
using CurrencyRateBattleServer.Dal.Services.Interfaces;
using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Dto;
using CurrencyRateBattleServer.Infrastructure;
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
    private readonly ILogger<RateController> _logger;

    private readonly IRateService _rateService;

    private readonly IAccountService _accountService;

    private readonly ICurrencyStateService _currencyStateService;

    private readonly IPaymentService _paymentService;

    public RateController(ILogger<RateController> logger,
        IRateService rateService,
        IAccountService accountService,
        ICurrencyStateService currencyStateService,
        IPaymentService paymentService)
    {
        _logger = logger;
        _rateService = rateService;
        _accountService = accountService;
        _currencyStateService = currencyStateService;
        _paymentService = paymentService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Room>>> GetRatesAsync(bool? isActive, string? currencyCode)
    {
        _logger.LogDebug("List of rates are retrieving.");
        var rates = await _rateService.GetRatesAsync(isActive, currencyCode);
        return Ok(rates);
    }

    [HttpGet("get-user-bets")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> GetUserBetsAsync()
    {
        _logger.LogDebug($"{nameof(GetUserBetsAsync)},  was caused.");
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
    public async Task<IActionResult> GetUsersRatingAsync()
    {
        _logger.LogDebug($"{nameof(GetUsersRatingAsync)},  was caused.");

        return Ok(await _rateService.GetUsersRatingAsync());
    }

    [HttpGet("get-currency-rates")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> GetCurrencyRates()
    {
        _logger.LogDebug($"{nameof(GetCurrencyRates)},  was caused.");

        var currencyState = await _currencyStateService.GetCurrencyStateAsync();
        return Ok(currencyState);
    }

    [HttpPost("make-bet")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreateRateAsync([FromBody] RateDto rateToCreate)
    {
        _logger.LogDebug("New rate creation is triggerred.");

        if (!ModelState.IsValid)
            return BadRequest("Wrong data");

        try
        {
            var userId = _accountService.GetGuidFromRequest(HttpContext);

            if (userId is null)
                return BadRequest("Incorrect data");

            var account = await _accountService.GetAccountByUserIdAsync(userId);

            if (account is null)
                return BadRequest("Incorrect data");

            if (!await _paymentService.WritingOffMoneyAsync(account.Id, rateToCreate.Amount))
                return Conflict("Payment processing error");

            var currencyId = await _currencyStateService.GetCurrencyIdByRoomIdAsync(rateToCreate.RoomId);

            if (currencyId == Guid.Empty)
                return BadRequest("Incorrect data");

            var rate = await _rateService.CreateRateAsync(rateToCreate, account.Id, currencyId);

            _logger.LogInformation("Rate has been created successfully ({Id})", rate.Id);
            return Ok(rate);
        }
        catch (GeneralException ex)
        {
            return BadRequest(new {message = ex.Message});
        }
        catch (DbUpdateException)
        {
            _logger.LogDebug("An unexpected error occurred during the attempt to create a rate in the DB.");
            return BadRequest("An unexpected error occurred. Please try again.");
        }
    }
}
