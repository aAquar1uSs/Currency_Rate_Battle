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

    private readonly IAccountService _accountService;

    private readonly IUserService _userService;
    
    private readonly IJwtManager _jwtManager;
    
    public LoginHandler(ILogger<LoginHandler> logger, IAccountService accountService, IJwtManager jwtManager,
        IUserService userService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
        _jwtManager = jwtManager ?? throw new ArgumentNullException(nameof(jwtManager));
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
    }
    
    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(LoginHandler)} was caused.");
        var userResult = User.Create(request.UserDto.Email, request.UserDto.Password);

        if (userResult.IsFailure)
        {
            _logger.LogWarning(userResult.Error);
            return Result.Failure<LoginResponse>(userResult.Error);   
        }

        var maybeUser = await _userService.FindAsync(userResult.Value);

        if (maybeUser is null)
        {
            _logger.LogWarning("User with such parameters doesn't exist");
            return Result.Failure<LoginResponse>("User with such parameters doesn't exist");   
        }

        var tokens = _jwtManager.Authenticate(maybeUser);

        return new LoginResponse { Tokens = tokens };
    }
}
