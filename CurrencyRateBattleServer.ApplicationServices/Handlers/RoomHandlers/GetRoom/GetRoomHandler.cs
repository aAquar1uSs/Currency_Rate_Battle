﻿using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.Dal.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.RoomHandlers.GetRoom;

public class GetRoomHandler : IRequestHandler<GetRoomCommand, Result<GetRoomResponse>>
{
    private readonly ILogger<GetRoomHandler> _logger;

    private readonly IRoomRepository _roomRepository;

    public GetRoomHandler(ILogger<GetRoomHandler> logger, IRoomRepository roomRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _roomRepository = roomRepository ?? throw new ArgumentNullException(nameof(roomRepository));
    }

    public async Task<Result<GetRoomResponse>> Handle(GetRoomCommand request, CancellationToken cancellationToken)
    {
        //ToDo Create method which been get CountRate etc...
        _logger.LogDebug($"{nameof(GetRoomHandler)} was caused. Start processing.");

        var rooms = await _roomRepository.GetRoomsAsync(request.IsClosed);

        return new GetRoomResponse {Rooms = rooms.ToDto()};
    }
}
