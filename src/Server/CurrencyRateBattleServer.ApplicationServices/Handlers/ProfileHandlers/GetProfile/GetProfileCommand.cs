using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.Domain.Entities.Errors;
using MediatR;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.ProfileHandlers.GetProfile;

public class GetProfileCommand : IRequest<Result<GetProfileResponse, Error>>
{
    public string UserEmail { get; set; }
}
