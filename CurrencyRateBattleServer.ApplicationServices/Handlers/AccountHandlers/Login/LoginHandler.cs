using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.ApplicationServices.Infrastructure.JwtManager.Interfaces;
using CurrencyRateBattleServer.Dal.Services.Interfaces;
using CurrencyRateBattleServer.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.AccountHandlers.Login;

public class LoginHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    private readonly ILogger<LoginHandler> _logger;
    private readonly IAccountRepository _accountRepository;
    private readonly IJwtManager _jwtManager;

    public LoginHandler(ILogger<LoginHandler> logger, IAccountRepository accountRepository, IJwtManager jwtManager)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        _jwtManager = jwtManager ?? throw new ArgumentNullException(nameof(jwtManager));
    }

    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var userResult = User.Create(request.UserDto.Email, request.UserDto.Password);

        if (userResult.IsFailure)
            return Result.Failure<LoginResponse>(userResult.Error);

        var maybeUser = await _accountRepository.GetUserAsync(userResult.Value);

        if (maybeUser is null)
        {
            _logger.LogInformation("User with such parameters doesn't exist");
            return Result.Failure<LoginResponse>("User with such parameters doesn't exist");   
        }

        var tokens = _jwtManager.Authenticate(maybeUser);

        return new LoginResponse { Tokens = tokens };
    }
}
