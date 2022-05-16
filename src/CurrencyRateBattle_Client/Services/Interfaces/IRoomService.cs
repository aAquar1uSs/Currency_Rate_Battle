﻿using CRBClient.Models;

namespace CRBClient.Services.Interfaces;
public interface IRoomService
{
    public Task<IEnumerable<RoomViewModel>> GetRooms();
}