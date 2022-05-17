using CRBClient.Models;

namespace CRBClient.Services.Interfaces;
public interface IUserService
{
    string GetUserInfo();

    string UpdateUserInfo();

    Task RegisterUserAsync(UserViewModel user);

    Task LoginUserAsync(UserViewModel user);
}
