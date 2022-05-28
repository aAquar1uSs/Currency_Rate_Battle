using CRBClient.Models;

namespace CRBClient.Services.Interfaces;
public interface IUserService
{
    Task RegisterUserAsync(UserViewModel user);

    Task LoginUserAsync(UserViewModel user);

    Task<AccountInfoViewModel> GetAccountInfoAsync();

    Task<List<AccountHistoryViewModel>> GetAccountHistoryAsync();

    Task<string> GetUserBalanceAsync();

    Task<decimal> GetUserBalanceDecimalAsync();

    void Logout();
}
