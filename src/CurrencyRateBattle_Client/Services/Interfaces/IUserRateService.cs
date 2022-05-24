using CRBClient.Models;

namespace CRBClient.Services.Interfaces;
public interface IUserRateService
{
    Task<List<BetViewModel>> GetUserRates();
    public string InsertUserRate();
    public string UpdateUserRate();
    public string DeleteUserRate();
}
