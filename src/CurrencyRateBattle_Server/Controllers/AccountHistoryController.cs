using System.Net;
using CurrencyRateBattleServer.ApplicationServices.Dto;
using CurrencyRateBattleServer.ApplicationServices.Handlers.HistoryHandlers.GetAccountHistory;
using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    public async Task<IActionResult> GetAccountHistoryAsync()
    {
        _logger.LogDebug($"{nameof(GetAccountHistoryAsync)} was triggered.");

        var command = new GetAccountHistoryCommand { UserId = GuidHelper.GetGuidFromRequest(HttpContext) };

        var response = await _mediator.Send(command);

        if (response.IsFailure)
            return BadRequest(response.Error);

        return Ok(response.Value.AccountHistories);
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> CreateNewAccountHistory([FromBody] AccountHistoryDto historyDto)
    {
        _logger.LogDebug($"{nameof(CreateNewAccountHistory)}, was caused");

        if (!ModelState.IsValid)
            return BadRequest("Wrong data");

        try
        {
            var userId = GuidHelper.GetGuidFromRequest(HttpContext);
            if (userId is null)
                return BadRequest();

            Room? room = null;
            var account = await _accountService.GetAccountByUserIdAsync(userId);

            if (account is null)
                return BadRequest();

            if (historyDto.RoomId is not null)
                room = await _roomService.GetRoomByIdAsync((Guid)historyDto.RoomId);

            if (account != null)
                await _historyService.CreateHistoryAsync(room, account, historyDto);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError("{Msg}", ex.Message);
            return BadRequest();
        }

        _logger.LogDebug("Account history successfully added.");
        return Ok();
    }
}
