using CRBClient.Models;

namespace CRBClient.Services.Interfaces;
public interface IUserService
{
    string GetUserInfo();

    string UpdateUserInfo();

    string RegisterUser();

    Task LoginUserAsync(UserViewModel user);
}
