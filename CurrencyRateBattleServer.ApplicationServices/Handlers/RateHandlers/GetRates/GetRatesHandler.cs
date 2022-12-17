using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.ApplicationServices.Converters;
using CurrencyRateBattleServer.ApplicationServices.Handlers.RoomHandlers.GetRoom;
using CurrencyRateBattleServer.Dal.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.RateHandlers.GetRates;

public class GetRatesHandler : IRequestHandler<GetRatesCommand, Result<GetRatesResponse>>
{
    private readonly ILogger<GetRatesHandler> _logger;

    private readonly IRateRepository _rateRepository;

    public GetRatesHandler(ILogger<GetRatesHandler> logger, IRateRepository rateRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _rateRepository = rateRepository ?? throw new ArgumentNullException(nameof(rateRepository));
    }

    public async Task<Result<GetRatesResponse>> Handle(GetRatesCommand request, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(GetRoomHandler)} was caused. Start processing.");

        var rates = await _rateRepository.GetRatesAsync(request.IsActive, request.CurrencyCode);

        return new GetRatesResponse { Rates = rates.ToDto() };
    }
}
