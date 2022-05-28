using CurrencyRateBattleServer.Dto;
using CurrencyRateBattleServer.Models;

namespace CurrencyRateBattleServer.Services.Interfaces;

public interface IAccountService
{
    /// <summary>
    /// Verifies the presence of a user , if any, gives his unique token <see cref="Tokens"/>;
    /// </summary>
    /// <param name="userData"><see cref="UserDto"/></param>
    /// <returns>
    ///the task result contains <see cref="Tokens"/>
    /// </returns>
    public Task<Tokens?> GetUserAsync(UserDto userData);

    /// <summary>
    /// Creates new account and gives his unique token <see cref="Tokens"/>;
    /// </summary>
    /// <param name="userData"><see cref="UserDto"/></param>
    /// <returns>
    ///the task result contains <see cref="Tokens"/>;
    /// </returns>
    /// <exception cref="Helpers.GeneralException"> Throws if such user is already taken;</exception>
    public Task<Tokens?> CreateUserAsync(UserDto userData);

    /// <summary>
    /// Gets account info from database by user id;
    /// </summary>
    /// <param name="id"><see cref="User"/> id;</param>
    /// <returns>
    ///the task result contains <see cref="AccountInfoDto"/>;
    /// </returns>
    public Task<AccountInfoDto?> GetAccountInfoByUserIdAsync(Guid id);

    /// <summary>
    /// Get account model from database by user id;
    /// </summary>
    /// <param name="userId"><see cref="User"/> id;</param>
    /// <returns>
    ///the task result contains <see cref="Account"/>;
    /// </returns>
    public Task<Account?> GetAccountByUserIdAsync(Guid? userId);

    /// <summary>
    /// Receives a token from a session and pulls user id from it;
    /// </summary>
    /// <param name="context"><see cref="HttpContext"/> To get a session and get a token out of there;</param>
    /// <returns>
    ///the task result contains <see cref="Guid"/> - user id;
    /// </returns>
    public Guid? GetGuidFromRequest(HttpContext context);

}
