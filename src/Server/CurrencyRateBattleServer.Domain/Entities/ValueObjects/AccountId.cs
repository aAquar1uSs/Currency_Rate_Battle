using CSharpFunctionalExtensions;

namespace CurrencyRateBattleServer.Domain.Entities.ValueObjects;

public class AccountId : CustomId
{
    private AccountId(Guid id) : base(id)
    {
       
    }

    public static Result<AccountId> TryCreate(Guid Id)
    {
        if (Id == Guid.Empty)
            Result.Failure<AccountId>("Id can not be empty");

        return new AccountId(Id);
    }

    public static AccountId Create(Guid id) => new(id);

    public static AccountId GenerateId() => new AccountId(Guid.NewGuid());
}
