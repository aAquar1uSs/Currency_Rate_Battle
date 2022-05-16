using CRBClient.Models;

namespace CRBClient.Services.Interfaces;
public interface IUserRateService
{
    public string GetUserRates();
    public string InsertUserRate();
    public string UpdateUserRate();
    public string DeleteUserRate();
}
