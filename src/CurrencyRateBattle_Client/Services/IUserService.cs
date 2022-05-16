using CRBClient.Models;

namespace CRBClient.Services;

public interface IUserService
{
    Task LoginAsync(UserViewModel user);

    Task RegistrationAsync(UserViewModel user);
}
