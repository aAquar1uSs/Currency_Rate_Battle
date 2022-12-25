﻿using CurrencyRateBattleServer.Dal.Entities;
using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;

namespace CurrencyRateBattleServer.Dal.Repositories.Interfaces;

public interface IRateRepository
{
    Task CreateAsync(Rate rate, CancellationToken cancellationToken);

    Task<Rate[]> GetRateByRoomIdsAsync(RoomId[] roomIds, CancellationToken cancellationToken);

    Task DeleteRateAsync(Rate rateToDelete);

    Task UpdateRateByRoomIdAsync(Rate[] updatedRate, CancellationToken cancellationToken);

    Task<Rate[]> FindAsync(bool? isActive, string? currencyName, CancellationToken cancellationToken);
}