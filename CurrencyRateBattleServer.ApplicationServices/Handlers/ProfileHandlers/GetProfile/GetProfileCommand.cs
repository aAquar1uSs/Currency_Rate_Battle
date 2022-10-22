using CSharpFunctionalExtensions;
using MediatR;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.ProfileHandlers.GetProfile;

public class GetProfileCommand : IRequest<Result<GetProfileResponse>>
{
    public Guid? UserId { get; set; }
}
