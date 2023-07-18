using AccountManager.Domain;

namespace AccountManager.ApplicationServices.Infrastructure.JWTManager;

public interface IJwtManager
{
    Tokens Authenticate(User user);
}
