using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.Domain.Entities.Errors;
using MediatR;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.HistoryHandlers.CreateAccountHistory;

public class CreateHistoryCommand : IRequest<Maybe<Error>>
{
    public CreateHistoryCommand(string userEmail, Guid? roomId, DateTime date, decimal amount,
        bool isCredit)
    {
        UserEmail = userEmail;
        RoomId = roomId;
        Date = date;
        Amount = amount;
        IsCredit = isCredit;
    }
    
    public string UserEmail { get; }

    public Guid? RoomId { get; set; }

    public DateTime Date { get; set; }

    public decimal Amount { get; set; }
    
    public bool IsCredit { get; set; }
}
