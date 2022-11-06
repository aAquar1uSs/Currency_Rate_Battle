﻿using System.Net;
using CurrencyRateBattleServer.ApplicationServices.Dto;
using CurrencyRateBattleServer.ApplicationServices.Handlers.RateHandlers.GetRates;
using CurrencyRateBattleServer.ApplicationServices.Handlers.RateHandlers.GetUserBets;
using CurrencyRateBattleServer.ApplicationServices.Handlers.RateHandlers.MakeBetHandler;
using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Dto;
using CurrencyRateBattleServer.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyRateBattleServer.Controllers;

[Route("api/rates")]
[ApiController]
[Authorize]
public class RateController : ControllerBase
{
    private readonly ILogger<RateController> _logger;

    private readonly IMediator _mediator;

    public RateController(ILogger<RateController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<Room>>> GetRatesAsync(bool? isActive, string? currencyCode)
    {
        _logger.LogDebug("List of rates are retrieving.");
        var command = new GetRatesCommand {IsActive = isActive, CurrencyCode = currencyCode};

        var response = await _mediator.Send(command);

        return Ok(response.Value.Rates);
    }

    [HttpGet("get-user-bets")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> GetUserBetsAsync()
    {
        _logger.LogDebug($"{nameof(GetUserBetsAsync)},  was caused.");
        var userId = GuidHelper.GetGuidFromRequest(HttpContext);
        if (userId is null)
            return BadRequest();

        var command = new GetUserBetsCommand {UserId = (Guid)userId};

        var response = await _mediator.Send(command);

        if (response.IsFailure)
            return BadRequest(response.Error);

        return Ok(response.Value.Bets);
    }

    [HttpPost("make-bet")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreateRateAsync([FromBody] RateDto rateToCreate)
    {
        _logger.LogDebug("New rate creation is triggerred.");

        var userId = GuidHelper.GetGuidFromRequest(HttpContext);

        if (userId is null)
            return BadRequest("Incorrect data");

        var command = new MakeBetCommand { RateToCreate = rateToCreate, UserId = (Guid)userId };

        var response = await _mediator.Send(command);

        if (response.IsFailure)
            return BadRequest(response.Error);

        return Ok(response.Value.Rate);
    }

}
