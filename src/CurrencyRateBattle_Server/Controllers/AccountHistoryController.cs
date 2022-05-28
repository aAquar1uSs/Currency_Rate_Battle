using System.Net;
using CurrencyRateBattleServer.Dto;
using CurrencyRateBattleServer.Models;
using CurrencyRateBattleServer.Services.Interfaces;
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

    private readonly IAccountHistoryService _historyService;

    private readonly IRoomService _roomService;

    private readonly IAccountService _accountService;

    public AccountHistoryController(ILogger<AccountHistoryController> logger,
        IAccountHistoryService historyService,
        IRoomService roomService,
        IAccountService accountService)
    {
        _logger = logger;
        _historyService = historyService;
        _roomService = roomService;
        _accountService = accountService;
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> GetAccountHistoryAsync()
    {
        var userId = _accountService.GetGuidFromRequest(HttpContext);
        if (userId is null)
            return BadRequest();

        var account = await _accountService.GetAccountByUserIdAsync(userId);
        if (account is null)
            return BadRequest();

        var history = await _historyService.GetAccountHistoryByAccountId(account.Id);

        return Ok(history);
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> CreateNewAccountHistory([FromBody] AccountHistoryDto historyDto)
    {
        try
        {
            var userId = _accountService.GetGuidFromRequest(HttpContext);
            if (userId is null)
                return BadRequest();

            Room? room = null;
            var account = await _accountService.GetAccountByUserIdAsync(userId);

            if (account is null)
                return BadRequest();

            if (historyDto.RoomId is not null)
                room = await _roomService.GetRoomByIdAsync((Guid)historyDto.RoomId);

            await _historyService.CreateHistoryAsync(room, account, historyDto);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest();
        }

        return Ok();
    }
}
