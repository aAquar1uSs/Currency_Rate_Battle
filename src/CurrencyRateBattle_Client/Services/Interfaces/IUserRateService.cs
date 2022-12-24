using CRBClient.Models;

namespace CRBClient.Services.Interfaces;
public interface IUserRateService
{
    Task<List<BetViewModel>> GetUserRates(CancellationToken cancellationToken);
    Task InsertUserRateAsync(RateViewModel rateViewModel, CancellationToken cancellationToken);
}
