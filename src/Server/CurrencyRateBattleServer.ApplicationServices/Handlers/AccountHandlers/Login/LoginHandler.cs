using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.ApplicationServices.Infrastructure.JwtManager.Interfaces;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Domain.Entities.Errors;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.AccountHandlers.Login;

public class LoginHandler : IRequestHandler<LoginCommand, Result<LoginResponse, Error>>
{
    private readonly ILogger<LoginHandler> _logger;
    private readonly IUserRepository _userRepository;
    private readonly IJwtManager _jwtManager;

    public LoginHandler(ILogger<LoginHandler> logger, IUserRepository userRepository, IJwtManager jwtManager)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _jwtManager = jwtManager ?? throw new ArgumentNullException(nameof(jwtManager));
    }

    public async Task<Result<LoginResponse, Error>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var emailResult = Email.TryCreate(request.Email);
        if (emailResult.IsFailure)
            return new PlayerValidationError("email_not_valid", emailResult.Error);

        var passwordResult = Password.TryCreate(request.Password);
        if (passwordResult.IsFailure)
            return new PlayerValidationError("password_not_valid", passwordResult.Error);

        var maybeUser = await _userRepository.Get(emailResult.Value, passwordResult.Value, cancellationToken);

        if (maybeUser is null)
        {
            _logger.LogInformation("User with such parameters doesn't exist");
            return PlayerValidationError.UserNotFound;
        }

        var tokens = _jwtManager.Authenticate(maybeUser);

        return new LoginResponse { Tokens = tokens };
    }
}
