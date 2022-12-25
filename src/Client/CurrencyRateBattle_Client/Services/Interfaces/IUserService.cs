using CRBClient.Models;

namespace CRBClient.Services.Interfaces;
public interface IUserService
{
    Task RegisterUserAsync(UserViewModel user, CancellationToken cancellationToken);

    Task LoginUserAsync(UserViewModel user, CancellationToken cancellationToken);

    Task<AccountInfoViewModel> GetAccountInfoAsync(CancellationToken cancellationToken);

    Task<List<AccountHistoryViewModel>> GetAccountHistoryAsync(CancellationToken cancellationToken);

    Task<string> GetUserBalanceAsync(CancellationToken cancellationToken);

    Task<decimal> GetUserBalanceDecimalAsync(CancellationToken cancellationToken);

    void Logout();
}
