using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.ApplicationServices.Infrastructure.JwtManager.Interfaces;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using CurrencyRateBattleServer.Dal.Services.Interfaces;
using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.AccountHandlers.Login;

public class LoginHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    private readonly ILogger<LoginHandler> _logger;
    private readonly IUserRepository _userRepository;
    private readonly IJwtManager _jwtManager;

    public LoginHandler(ILogger<LoginHandler> logger, IAccountRepository accountRepository,
        IUserRepository userRepository, IJwtManager jwtManager)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _jwtManager = jwtManager ?? throw new ArgumentNullException(nameof(jwtManager));
    }

    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var customId = UserId.GenerateId();
        var userResult = User.TryCreate(customId.Id, request.UserDto.Email, request.UserDto.Password, null);

        if (userResult.IsFailure)
            return Result.Failure<LoginResponse>(userResult.Error);

        var maybeUser = await _userRepository.GetAsync(userResult.Value, cancellationToken);

        if (maybeUser is null)
        {
            _logger.LogInformation("User with such parameters doesn't exist");
            return Result.Failure<LoginResponse>("User with such parameters doesn't exist");
        }

        var tokens = _jwtManager.Authenticate(maybeUser);

        return new LoginResponse { Tokens = tokens };
    }
}
