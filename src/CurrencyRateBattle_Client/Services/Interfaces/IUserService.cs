using CRBClient.Models;

namespace CRBClient.Services.Interfaces;
public interface IUserService
{
    string UpdateUserInfo();

    Task RegisterUserAsync(UserViewModel user);

    Task LoginUserAsync(UserViewModel user);

    Task<AccountInfoViewModel> GetAccountInfoAsync();

    Task<List<AccountHistoryViewModel>> GetAccountHistoryAsync();

    Task<string> GetUserBalanceAsync();

    public void Logout();
}
