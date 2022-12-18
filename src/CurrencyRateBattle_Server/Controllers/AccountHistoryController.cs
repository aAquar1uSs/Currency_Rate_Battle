﻿using System.Net;
using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.ApplicationServices.Dto;
using CurrencyRateBattleServer.ApplicationServices.Handlers.HistoryHandlers.CreateAccountHistory;
using CurrencyRateBattleServer.ApplicationServices.Handlers.HistoryHandlers.GetAccountHistory;
using CurrencyRateBattleServer.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyRateBattleServer.Controllers;

[Route("api/history")]
[ApiController]
[Authorize]
public class AccountHistoryController : ControllerBase
{
    private readonly ILogger<AccountHistoryController> _logger;
    private readonly IMediator _mediator;

    public AccountHistoryController(ILogger<AccountHistoryController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> GetAccountHistoryAsync(CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(GetAccountHistoryAsync)} was triggered.");
        var userId = GuidHelper.GetGuidFromRequest(HttpContext);
        if (userId is null)
            return BadRequest();
        
        var command = new GetAccountHistoryCommand { UserId = userId.Value };

        var (_, isFailure, value, error) = await _mediator.Send(command, cancellationToken);

        if (isFailure)
            return BadRequest(error);

        return Ok(value.AccountHistories);
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> CreateNewAccountHistory([FromBody] AccountHistoryDto historyDto, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(CreateNewAccountHistory)}, was caused");

        var userId = GuidHelper.GetGuidFromRequest(HttpContext);
        if (userId is null)
            return BadRequest();

        var command = new CreateHistoryCommand { UserId = userId.Value, AccountHistory = historyDto };

        var (_, isFailure, _, error) = await _mediator.Send(command, cancellationToken);

        if (isFailure)
            return BadRequest(error);

        _logger.LogDebug("Account history successfully added.");
        return Ok();
    }
}
