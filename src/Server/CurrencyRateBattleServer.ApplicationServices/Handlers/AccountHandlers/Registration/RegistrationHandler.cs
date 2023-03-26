using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.ApplicationServices.Dto;
using CurrencyRateBattleServer.ApplicationServices.Handlers.HistoryHandlers.CreateAccountHistory;
using CurrencyRateBattleServer.ApplicationServices.Infrastructure;
using CurrencyRateBattleServer.ApplicationServices.Infrastructure.JwtManager.Interfaces;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Domain.Entities.Errors;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.AccountHandlers.Registration;

public class RegistrationHandler : IRequestHandler<RegistrationCommand, Result<RegistrationResponse, Error>>
{
    private readonly ILogger<RegistrationHandler> _logger;
    private readonly IAccountRepository _accountRepository;
    private readonly IUserRepository _userRepository;
    private readonly WebServerOptions _options;
    private readonly IJwtManager _jwtManager;
    private readonly IMediator _mediator;

    public RegistrationHandler(ILogger<RegistrationHandler> logger, IAccountRepository accountRepository, 
        IOptions<WebServerOptions> options, IUserRepository userRepository, IJwtManager jwtManager, IMediator mediator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        _options = options.Value ?? throw new ArgumentNullException(nameof(options));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _jwtManager = jwtManager ?? throw new ArgumentNullException(nameof(jwtManager));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task<Result<RegistrationResponse, Error>> Handle(RegistrationCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(RegistrationHandler)} was caused.");

        var emailResult = Email.TryCreate(request.Email);
        if (emailResult.IsFailure)
            return new PlayerValidationError("email_not_valid", emailResult.Error);

        var passwordResult = Password.TryCreate(request.Password);
        if (passwordResult.IsFailure)
            return new PlayerValidationError("password_not_valid", passwordResult.Error);
        
        var maybeUser = await _userRepository.Get(emailResult.Value, passwordResult.Value, cancellationToken);

        if (maybeUser is not null)
            return PlayerValidationError.UserAlreadyExist;
        

        var amountResult = Amount.TryCreate(_options.RegistrationReward);
        if (amountResult.IsFailure)
            return new MoneyValidationError("amount_not_valid", amountResult.Error);

        var customAccountId = AccountId.GenerateId();
        var account = Account.TryCreateNewAccount(customAccountId, emailResult.Value, amountResult.Value);
        
        var user = User.Create(request.Email, request.Password);
        
        await _userRepository.Create(user, cancellationToken);

        await _accountRepository.Create(account, cancellationToken);
        
        var command = new CreateHistoryCommand(user.Email.Value, null, DateTime.UtcNow, amountResult.Value.Value, true);
        _ = await _mediator.Send(command, cancellationToken);
        
        return new RegistrationResponse { Tokens = _jwtManager.Authenticate(user) };
    }
}
