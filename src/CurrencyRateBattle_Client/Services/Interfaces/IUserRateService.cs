using CRBClient.Models;

namespace CRBClient.Services.Interfaces;
public interface IUserRateService
{
    Task<List<BetViewModel>> GetUserRates();
    Task InsertUserRateAsync(RateViewModel rateViewModel);
    public string UpdateUserRate();
    public string DeleteUserRate();
}
