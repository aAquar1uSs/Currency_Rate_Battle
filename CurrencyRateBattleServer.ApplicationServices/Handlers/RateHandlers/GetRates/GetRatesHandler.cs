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

    private readonly IRateService _rateService;

    public GetRatesHandler(ILogger<GetRatesHandler> logger, IRateService rateService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _rateService = rateService ?? throw new ArgumentNullException(nameof(rateService));
    }

    public async Task<Result<GetRatesResponse>> Handle(GetRatesCommand request, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(GetRoomHandler)} was caused. Start processing.");

        var rates = await _rateService.GetRatesAsync(request.IsActive, request.CurrencyCode);

        return new GetRatesResponse { Rates = rates.ToDto() };
    }
}
