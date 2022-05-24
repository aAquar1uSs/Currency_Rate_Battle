using CurrencyRateBattleServer.Data;
using CurrencyRateBattleServer.Dto;
using CurrencyRateBattleServer.Services.Interfaces;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace CurrencyRateBattleServer.Services;

public class CurrencyStateService : ICurrencyStateService
{
    private const string NBU_API = "https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?json";

    private readonly ILogger<CurrencyStateService> _logger;

    private List<CurrencyStateDto>? _rateStorage;

    private readonly IServiceScopeFactory _scopeFactory;

    private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);

    public CurrencyStateService(ILogger<CurrencyStateService> logger,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        _rateStorage = new List<CurrencyStateDto>();
    }

    public async Task PrepareUpdateCurrencyRateAsync()
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CurrencyRateBattleContext>();

        await GetCurrencyRatesFromNbuApiAsync();

        foreach (var currState in dbContext.CurrencyStates)
        {
            await UpdateCurrencyRateByIdAsync(currState.CurrencyId);
        }
    }

    public async Task GetCurrencyRatesFromNbuApiAsync()
    {
        using var client = new HttpClient();
        try
        {
            client.BaseAddress = new Uri("https://localhost:7255");

            var stream = await client.GetStreamAsync(NBU_API);
            _rateStorage = await JsonSerializer.DeserializeAsync<List<CurrencyStateDto>>(stream);

            if (_rateStorage is null)
                throw new ArgumentException(nameof(_rateStorage));
        }
        catch (HttpRequestException httpRequestException)
        {
            _logger.LogError(httpRequestException.StackTrace);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex.Message);
        }
    }

    public async Task UpdateCurrencyRateByIdAsync(Guid currId)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CurrencyRateBattleContext>();

        var currencyName = (await db.Currencies
            .FirstOrDefaultAsync(curr => curr.Id == currId))?.CurrencyName;

        var currencyDto = _rateStorage.FirstOrDefault(curr => curr.Currency.Equals(currencyName,
            StringComparison.Ordinal));

        var currentDate = DateTime.ParseExact(DateTime.UtcNow.ToString("MM.dd.yyyy HH:00:00"),
            "MM.dd.yyyy HH:mm:ss", null);

        await _semaphoreSlim.WaitAsync();
        try
        {
            var currencyStates = db
                .CurrencyStates.Where(curr => curr.CurrencyId == currId);

            foreach (var curr in currencyStates)
            {
                curr.Date = currentDate;
                curr.CurrencyExchangeRate = Math.Round(currencyDto.Rate, 2);
            }

            await db.SaveChangesAsync();
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }
}
