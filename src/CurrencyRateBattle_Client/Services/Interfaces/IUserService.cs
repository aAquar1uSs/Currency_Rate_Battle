using CRBClient.Models;

namespace CRBClient.Services.Interfaces;
public interface IUserService
{
    public string GetUserInfo();
    public string UpdateUserInfo();
    public string RegisterUser();
}
